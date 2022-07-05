using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PhysicsCastVisualizers
{
    // [DisallowMultipleComponent]
    [AddComponentMenu("Physics Cast Visualizers/Shape Checks/Box Check")]
    public class BoxCheck : ShapeCheckVisualizer
    {
        [SerializeField] protected Vector3 size = Vector3.one;
        [SerializeField] private bool formFromScale;

        protected virtual void Start() 
        {
            if (formFromScale)
                size = transform.localScale;
        }

        protected override bool Cast()
        {
            switch (direction)
            {
                case CastDirection.Right:
                    directionAxisSize = size.x;
                    break;
                case CastDirection.Up:
                    directionAxisSize = size.y;
                    break;
                case CastDirection.Forward:
                    directionAxisSize = size.z;
                    break;
                case CastDirection.Left:
                    directionAxisSize = size.x;
                    break;
                case CastDirection.Down:
                    directionAxisSize = size.y;
                    break;
                case CastDirection.Back:
                    directionAxisSize = size.z;
                    break;
            }

            return Physics.CheckBox(CalculateCastPosition(directionAxisSize/2), size/2, rotation, collidingLayers, EvaluateTriggerDetection());
        }

        void OnDrawGizmos() 
        {
            if(Application.isPlaying && visualize && visualizeOverride)
            {
                Gizmos.color = hasHit ? Color.red : Color.green;
                Gizmos.DrawWireMesh(castMesh, relativePosition, rotation, size);
            }
            else if (!Application.isPlaying && visualizeOverride)
            {
                rotation = useParentRot ? transform.parent.rotation : transform.rotation;
                switch (direction)
                {
                    case CastDirection.Right:
                        directionAxisSize = size.x;
                        break;
                    case CastDirection.Up:
                        directionAxisSize = size.y;
                        break;
                    case CastDirection.Forward:
                        directionAxisSize = size.z;
                        break;
                    case CastDirection.Left:
                        directionAxisSize = size.x;
                        break;
                    case CastDirection.Down:
                        directionAxisSize = size.y;
                        break;
                    case CastDirection.Back:
                        directionAxisSize = size.z;
                        break;
                }

                Gizmos.DrawWireMesh(castMesh, CalculateCastPosition(directionAxisSize/2), rotation, size);
            }
        }
    }   
}


