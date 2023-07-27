using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace PhysCastVisualier
{
    public abstract class ShapeCheckVisualizer : CastVisualizer<bool>
    {
        [SerializeField] protected bool hasDirection;
        [SerializeField] protected Mesh castMesh;
        [SerializeField] protected Vector3 offset;
        [SerializeField] protected UnityEvent OnDetectionEnter;
        [SerializeField] protected UnityEvent OnDetectionExit;
        public event Action OnDetectionEnter_;
        public event Action OnDetectionExit_;

        protected override void AutoCast() 
        {
            bool hasHitNow = Cast();

            if (hasHitNow != hasHit)
            {
                if (hasHitNow) {
                    OnDetectionEnter?.Invoke();
                    OnDetectionEnter_?.Invoke();
                } else {
                    OnDetectionExit?.Invoke();
                    OnDetectionExit_?.Invoke();
                }
            }

            hasHit = hasHitNow;
        }

        protected abstract bool Cast();
        public override bool ManualCast()
        {
            base.ManualCast();
            hasHit = Cast();
            return hasHit;
        }

        protected Vector3 CalculateCastPosition(float directionBodySize)
        {
            rotationOffset = transform.rotation * (offset + GlobalCastDirections[(int)direction] * (directionBodySize) * Convert.ToInt32(hasDirection));
            relativePosition = transform.position + rotationOffset;
            return relativePosition;
        }
        
    }

}

