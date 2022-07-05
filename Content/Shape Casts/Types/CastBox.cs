using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsCastVisualizers
{
    [AddComponentMenu("Physics Cast Visualizers/Shape Casts/Cast Box")]
    public class CastBox : ShapeCastVisualizer
    {
        [VectorConstraint(nameof(direction)), SerializeField] private Vector3 extentSize  = Vector3.one / 2;

        private void Awake() => visualize = true;
        
        protected override void Cast()
        {
            visualize = true;
            hasHit = false;
            Vector3 castDirection = GetLocalCastDirection(direction);
            Vector3 posWOffset = transform.position + castDirection * directionOriginOffset;

            if (Physics.BoxCast(posWOffset, extentSize, castDirection, out hit, transform.rotation, maxDistance, collidingLayers, 
                detectTriggers ? QueryTriggerInteraction.Collide : QueryTriggerInteraction.Ignore))
            {
                for (int i = 0; i < targetTags.Length; i++)
                {
                    if (hit.transform.CompareTag(targetTags[i])) {
                        hasHit = true;
                        break;
                    }
                }    
            }

            // hasHit = Physics.BoxCast(posWOffset, extentSize, castDirection, out hit, transform.rotation, maxDistance, collidingLayers, 
            //     detectTriggers ? QueryTriggerInteraction.Collide : QueryTriggerInteraction.Ignore);

            if (visualizeOverride && visualize)
                DrawCube(castDirection * (maxDistance/2) + posWOffset, GetExtentSize(), transform.rotation, hasHit ? Color.red : Color.white); 
        }

        void OnDrawGizmos()
        {
            if (!Application.isPlaying && visualizeOverride)
            {
                Vector3 posWOffset = transform.position + GetLocalCastDirection(direction) * directionOriginOffset;
                DrawCube(GetLocalCastDirection(direction) * (maxDistance/2) + posWOffset, GetExtentSize(), transform.rotation, Color.white);         
            }
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

        public virtual RaycastHit? CastForward(float lenght, float width, float height)
        {
            this.maxDistance = lenght;
            autoCast = false;

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

            Cast();

            if (hasHit)
                return hit;

            return null;
        }

        public virtual RaycastHit? CastForward(float lenght)
        {
            this.maxDistance = lenght;
            autoCast = false;

            Cast();

            if (hasHit)
                return hit;

            return null;
        }
    }
   
}

