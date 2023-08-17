/* 
Copyright 2023 INF

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

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
        private Vector3 castEndPointWithOffset;


        protected override bool Cast()
        {
            casting = true;
            castTimeFrame = Time.frameCount;
            CalculateDirAndPos();

            if (Physics.SphereCast(posWOffset, radius, castDirection, out hitResult, maxDistance, collidingLayers, GetTriggerInteraction() )) 
                return CheckTags();

            return false;
        }

        public RaycastHit ManualCast( float newRadius = -1, float newDistance = -1 )
        {
            this.radius = newRadius == -1 ? this.radius : newRadius;
            this.maxDistance = newDistance == -1 ? this.maxDistance : newDistance;
            EventCheck(Cast());
            return hitResult;
        }

        public void SetRadius(float radius) => this.radius = radius;

    #region Visual Debug =============================================================================================================

        // shift the position of the point from the center of the sphere to the local direction where it's supposed to be along at
        // then multiplying it to the radius of the sphere to position it to the surface of the sphere
        // that essentially adding distance to the point to position it on the sphere surface
        private Vector3 GetLineStartPos(CastDirection directionPosShift) => posWOffset + (GetLocalCastDirection(directionPosShift) * radius);
        private Vector3 GetLineEndPos(CastDirection directionPosShift) => castEndPointWithOffset + (GetLocalCastDirection(directionPosShift) * radius);

        protected override void OnDrawGizmos()
        {
            if (!visualize)
                return;

            if (!casting)
                CalculateDirAndPos();

            castEndPointWithOffset = transform.position + castDirection * (directionOriginOffset + maxDistance);

            Gizmos.color = GetDebugColor();
            Gizmos.DrawWireSphere(posWOffset, radius); // origin sphere

            // add the radius in the calculation of the starting point of the line to shift it's pos to the edge of the sphere
            // and subtract it from the max distance on the ending point of the line to also position it to the edge of the max distance sphere
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
        }

    #endregion Visual Debug =============================================================================================================
    }

}

