using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PhysicsCastVisualizer
{
    // [DisallowMultipleComponent]
    [AddComponentMenu("Physics Cast Visualizer/Shape Checks/Box Check")]
    public class BoxCheck : ShapeCheckVisualizer
    {
        [SerializeField] protected Vector3 size = Vector3.one;
        [SerializeField] private bool formFromScale;
        private float directionAxisSize = 0;

        protected virtual void Start() 
        {
            if (formFromScale)
                size = transform.localScale;
        }

        private void SetDirectionAxisSize()
        {
            switch (direction)
            {
                case CastDirection.Left:
                case CastDirection.Right:
                    directionAxisSize = size.x;
                    break;
                case CastDirection.Up:
                case CastDirection.Down:
                    directionAxisSize = size.y;
                    break;
                case CastDirection.Forward:
                case CastDirection.Back:
                    directionAxisSize = size.z;
                    break;
            }
        }

        protected override bool Cast()
        {
            SetDirectionAxisSize();
            return Physics.CheckBox(CalculateCastPosition(directionAxisSize/2), size/2, transform.rotation, collidingLayers, EvaluateTriggerDetection());
        }

        protected override void OnDrawGizmos() 
        {
            base.OnDrawGizmos();
            if (!visualize)
                return;

            if(Application.isPlaying)
            {
                Gizmos.color = hasHit ? Color.red : Color.green;
                Gizmos.DrawWireMesh(castMesh, relativePosition, transform.rotation, size);
            }
            else if (!Application.isPlaying)
            {
                SetDirectionAxisSize();
                Gizmos.DrawWireMesh(castMesh, CalculateCastPosition(directionAxisSize/2), transform.rotation, size);
            }
        }
    }   
}


