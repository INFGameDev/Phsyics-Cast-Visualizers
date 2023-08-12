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
            bool hasHitNow = false;
            Vector3 castDirection = GetLocalCastDirection(direction);
            Vector3 posWOffset = transform.position + castDirection * directionOriginOffset;

            if (Physics.BoxCast(posWOffset, extentSize, castDirection, out hitResult, transform.rotation, maxDistance, collidingLayers, GetTriggerInteraction()))
            {
                hasHitNow = CheckTags();
            }
            return hasHitNow;
        }

        public RaycastHit ManualCast(
            Vector2 extents,
            CastDirection direction,
            float distance,
            LayerMask targetLayers,
            bool detectTriggers
        )
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

            this.direction = direction;
            this.maxDistance = distance;
            this.collidingLayers = targetLayers;
            this.GetTriggerInteraction(detectTriggers);

            EventCheck(Cast());
            return hitResult;
        }

        protected override void OnDrawGizmos()
        {
            if (!visualize)
                return;

            Vector3 castDirection = GetLocalCastDirection(direction);
            Vector3 posWOffset = transform.position + castDirection * directionOriginOffset;
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
    }

}

