using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysCastVisualier
{
    [AddComponentMenu("Physics Cast Visualizer/Shape Casts/Cast Box")]
    public class CastBox : ShapeCastVisualizer
    {
        [BoxDivider("Cast Box Properties")]
        [VectorConstraint(nameof(direction)), SerializeField] private Vector3 extentSize  = Vector3.one / 2;

        protected override bool Cast()
        {
            bool hasHitNow = false;
            Vector3 castDirection = GetLocalCastDirection(direction);
            Vector3 posWOffset = transform.position + castDirection * directionOriginOffset;

            if (Physics.BoxCast(posWOffset, extentSize, castDirection, out hit_, transform.rotation, maxDistance, collidingLayers, 
                GetTriggerInteraction()))
            {
                if (targetTags.Length > 0)
                {
                    for (int i = 0; i < targetTags.Length; i++)
                    {
                        if (hit_.transform.CompareTag(targetTags[i])) {
                            hasHitNow = true;
                            break;
                        }
                    }                     
                } else {
                    hasHitNow = true;                   
                }
            }

            // hasHit = Physics.BoxCast(posWOffset, extentSize, castDirection, out hit, transform.rotation, maxDistance, collidingLayers, 
            //     detectTriggers ? QueryTriggerInteraction.Collide : QueryTriggerInteraction.Ignore);
                
            return hasHitNow;
        }

        public virtual RaycastHit? ManualCast(float lenght, float width, float height)
        {
            base.ManualCast();
            this.maxDistance = lenght;

            switch (direction)
            {
                case CastDirection.Forward:
                case CastDirection.Back:
                    extentSize = new Vector3(width/2, height/2, 0);
                    break;
                case CastDirection.Right:
                case CastDirection.Left:
                    extentSize = new Vector3(0, width/2, height/2);
                    break;
                case CastDirection.Up:
                case CastDirection.Down:
                    extentSize = new Vector3(width/2, 0, height/2);
                    break;
            }

            hasHit = Cast();
            return hit;
        }

        public virtual RaycastHit? ManualCast(float lenght)
        { 
            base.ManualCast();
            this.maxDistance = lenght;
            hasHit = Cast();
            return hit;
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            if (!visualize)
                return;

            Vector3 castDirection = GetLocalCastDirection(direction);
            Vector3 posWOffset = transform.position + castDirection * directionOriginOffset;
            DrawCube(castDirection * (maxDistance/2) + posWOffset, GetExtentSize(), transform.rotation, hasHit ? hasHitColor : castColor);     
        }

        private Vector3 GetExtentSize()
        {
            Vector3 cubeExtent = extentSize;

            switch (direction)
            {
                case CastDirection.Right:
                case CastDirection.Left:
                    cubeExtent.x = maxDistance / 2;
                    break;
                case CastDirection.Up:
                case CastDirection.Down:
                    cubeExtent.y = maxDistance / 2;
                    break;
                case CastDirection.Forward:
                case CastDirection.Back:
                    cubeExtent.z = maxDistance / 2;
                    break;
            }

            return cubeExtent;
        }

        private readonly Vector3[] CubeVertexPoints = {
            new Vector3(1, 1, 1),
            new Vector3(1, 1, -1),
            new Vector3(1, -1, 1),
            new Vector3(1, -1, -1),
            new Vector3(-1, 1, 1),
            new Vector3(-1, 1, -1),
            new Vector3(-1, -1, 1),
            new Vector3(-1, -1, -1)
        };

        private void DrawCube(Vector3 center, Vector3 extent, Quaternion rotation, Color color)
        {
            Vector3[] points = new Vector3[8];
            for (int i = 0; i < points.Length; i++) {
                points[i] = rotation * Vector3.Scale(extent, CubeVertexPoints[i]) + center;
            }     

            Debug.DrawLine(points[0], points[1], color);
            Debug.DrawLine(points[0], points[2], color);
            Debug.DrawLine(points[0], points[4], color);

            Debug.DrawLine(points[7], points[6], color);
            Debug.DrawLine(points[7], points[5], color);
            Debug.DrawLine(points[7], points[3], color);

            Debug.DrawLine(points[1], points[3], color);
            Debug.DrawLine(points[1], points[5], color);

            Debug.DrawLine(points[2], points[3], color);
            Debug.DrawLine(points[2], points[6], color);

            Debug.DrawLine(points[4], points[5], color);
            Debug.DrawLine(points[4], points[6], color);   
        }
    }
   
}

