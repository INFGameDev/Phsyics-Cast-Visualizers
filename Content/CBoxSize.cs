using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PhysCastVisualier
{
    [Serializable]
    public class CBoxSize
    {
        public Vector3 size = Vector3.one;

        [HideInInspector] public float directionAxisSize = 0;

        public void SetDirectionAxisSize(CastDirection direction)
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

        public void PositiveInfinityClamp()
        {
            size.x = size.x.Clamp(0, Mathf.Infinity);
            size.y = size.y.Clamp(0, Mathf.Infinity);
            size.z = size.z.Clamp(0, Mathf.Infinity);
        }
    }


}

