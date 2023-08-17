/* 
Copyright (C) 2023 INF

This code is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License as published
by the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PhysCastVisualier
{
    [AddComponentMenu("Physics Cast Visualizer/Shape Casts/Cast Box")]
    public class CastBox : ShapeCastVisualizer
    {
        [BoxDivider("Cast Box Properties")]
        [VectorConstraint(nameof(direction)), SerializeField] private Vector3 extentSize = Vector3.one / 2;

        protected override bool Cast()
        {
            casting = true;
            castTimeFrame = Time.frameCount;
            CalculateDirAndPos();

            if (Physics.BoxCast(posWOffset, extentSize, castDirection, out hitResult, transform.rotation, maxDistance, collidingLayers, GetTriggerInteraction()))
                return CheckTags();

            return false;
        }

        public RaycastHit ManualCast( Vector2 extents, float newDistance = -1 )
        {
            SetExtentSize(extents);
            this.maxDistance = newDistance == -1 ? this.maxDistance : newDistance;
            EventCheck(Cast());
            return hitResult;
        }

        public void SetExtentSize(Vector2 extents)
        {
            switch (direction)
            {
                case CastDirection.Forward:
                case CastDirection.Back:
                    extentSize = new Vector3(extents.x, extents.y, 0);
                    break;
                case CastDirection.Right:
                case CastDirection.Left:
                    extentSize = new Vector3(0, extents.x, extents.y);
                    break;
                case CastDirection.Up:
                case CastDirection.Down:
                    extentSize = new Vector3(extents.x, 0, extents.y);
                    break;
            }
        }

        #region Visual Debug =============================================================================================================

        protected override void OnDrawGizmos()
        {
            if (!visualize)
                return;

            if (!casting)
                CalculateDirAndPos();

            DrawCube(castDirection * (maxDistance / 2) + posWOffset, GetExtentSize(), transform.rotation, GetDebugColor());
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
            for (int i = 0; i < points.Length; i++)
            {
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
        #endregion Visual Debug =============================================================================================================
    }

}

