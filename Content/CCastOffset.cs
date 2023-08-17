using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PhysCastVisualier
{
    [Serializable]
    public class CCastOffset
    {
        public Vector3 offset;
        [SerializeField, HideInInspector] private Vector3 rotationOffset;
        [HideInInspector] public Vector3 relativePosition;

        public void CalculateCastPosition(float directionBodySize, CastDirection direction, Transform transform)
        {
            int hasDirection = direction != CastDirection.None ? 1 : 0;
            rotationOffset = transform.rotation * (offset + ShapeCheckVisualizer.GlobalCastDirections[(int)direction] * (directionBodySize) * hasDirection);
            relativePosition = transform.position + rotationOffset;
        }
    }

}

