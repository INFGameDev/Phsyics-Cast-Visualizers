using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysCastVisualier
{
    [AddComponentMenu("Physics Cast Visualizer/Shape Casts/Cast Sphere")]
    public class CastSphere : ShapeCastVisualizer
    {
        [BoxDivider("Cast Sphere Properties")]
        [SerializeField] private float radius;
        private Vector3 vectorCastDirection;
        private Vector3 castOriginPointWithOffset;
        private Vector3 castEndPointWithOffset;
        protected override bool Cast()
        {
            casting = true;
            bool hasHitNow = false;
            Vector3 castDirection = GetLocalCastDirection(direction);
            Vector3 posWOffset = transform.position + castDirection * directionOriginOffset;

            if (Physics.SphereCast(posWOffset, radius, castDirection, out _hit, maxDistance, collidingLayers, GetTriggerInteraction() )) 
            {
                 hasHitNow = CheckTags();
            }

            return hasHitNow;
        }

        public RaycastHit? ManualCast(
            float radius, 
            CastVisualizer.CastDirection direction, 
            float distance, 
            LayerMask targetLayers, 
            bool detectTriggers
        )
        {
            this.radius = radius;
            this.direction = direction;
            this.maxDistance = distance;
            this.collidingLayers = targetLayers;
            this.GetTriggerInteraction(detectTriggers);

            EventCheck(Cast());
            return hit;
        }

        private Vector3 GetLineStartPos(CastDirection directionPosShift)
        {
            // shift the position of the point from the center of the sphere to the local direction where it's supposed to be along at
            // then multiplying it to the radius of the sphere to position it to the surface of the sphere
            // that essentially adding distance to the point to position it on the sphere surface
            return castOriginPointWithOffset + (GetLocalCastDirection(directionPosShift) * radius);
        }

        private Vector3 GetLineEndPos(CastDirection directionPosShift)
        {
            return castEndPointWithOffset + (GetLocalCastDirection(directionPosShift) * radius);
        }


        private void OnDrawGizmos()
        {
            if (!visualize)
                return;

            vectorCastDirection = GetLocalCastDirection(direction);
            castOriginPointWithOffset = transform.position + vectorCastDirection * directionOriginOffset;
            castEndPointWithOffset = transform.position + vectorCastDirection * (directionOriginOffset + maxDistance);
            Gizmos.color = hasHit && casting ? hasHitColor: castColor;
            
            Gizmos.DrawWireSphere(castOriginPointWithOffset, radius); // origin sphere

            // add the radius in the calculation of the starting point of the line to shift it's pos to the edge of the sphere
            // and subtract it from the max distance on the ending point of the line to also position it to the edge of the max distance sphere
            // Gizmos.DrawLine(posWOffset + (castDirection * radius), transform.position + castDirection * (directionOriginOffset + maxDistance - radius));

            CastDirection topDirPosShift = direction == CastDirection.Up || direction == CastDirection.Down ? CastDirection.Back : CastDirection.Up;
            CastDirection bottomDirPosShift = direction == CastDirection.Up || direction == CastDirection.Down ? CastDirection.Forward : CastDirection.Down;
            CastDirection leftDirPostShift = direction == CastDirection.Left || direction == CastDirection.Right ? CastDirection.Back : CastDirection.Left;
            CastDirection rightDirPostShift = direction == CastDirection.Left || direction == CastDirection.Right ? CastDirection.Forward : CastDirection.Right;

            // Local Top Line
            Gizmos.DrawLine(
                GetLineStartPos(topDirPosShift), 
                GetLineEndPos(topDirPosShift)
            );

            // Local Bottom Line
            Gizmos.DrawLine(
                GetLineStartPos(bottomDirPosShift), 
                GetLineEndPos(bottomDirPosShift)
            );

            // Local Left Line
            Gizmos.DrawLine(
                GetLineStartPos(leftDirPostShift), 
                GetLineEndPos(leftDirPostShift)
            );

            // Local Right Line
            Gizmos.DrawLine(
                GetLineStartPos(rightDirPostShift), 
                GetLineEndPos(rightDirPostShift)
            );

            // max distance sphere
            Gizmos.DrawWireSphere(castEndPointWithOffset, radius);
            Gizmos.color = default;
            casting = false; 
        }
    }

}

