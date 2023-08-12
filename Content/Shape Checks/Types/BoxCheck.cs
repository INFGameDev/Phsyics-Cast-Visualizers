using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PhysCastVisualier
{
    // [DisallowMultipleComponent]
    [AddComponentMenu("Physics Cast Visualizer/Shape Checks/Box Check")]
    public class BoxCheck : ShapeCheckVisualizer
    {
        [BoxDivider("Box Check Properties")]
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
            casting = true;
            castTimeFrame = Time.frameCount;
            SetDirectionAxisSize();
            return Physics.CheckBox(CalculateCastPosition(directionAxisSize / 2), size / 2, transform.rotation, collidingLayers, GetTriggerInteraction());
        }

        void OnValidate() {
            size.x = size.x.Clamp(0, Mathf.Infinity);
            size.y = size.y.Clamp(0, Mathf.Infinity);
            size.z = size.z.Clamp(0, Mathf.Infinity);
        }

        protected override void OnDrawGizmos()
        {
            if (!visualize)
                return;

            if (!Application.isPlaying)
                SetDirectionAxisSize();

            Gizmos.color = GetDebugColor();
            Gizmos.DrawWireMesh(castMesh, CalculateCastPosition(directionAxisSize / 2), transform.rotation, size);
        }
    }
}


