using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PhysicsCastVisualizers
{
    public abstract class ShapeCheckVisualizer : CastVisualizer
    {
        [SerializeField] protected bool hasDirection;
        [SerializeField] protected Mesh castMesh;
        [SerializeField] protected Vector3 offset;
        protected float directionAxisSize = 0;
        public event Action OnDetectionEnter;
        public event Action OnDetectionExit;
        protected virtual void Update() 
        {
            rotation = useParentRot ? transform.parent.rotation : transform.rotation;

            if (autoCast)
                AutoCast();
        }

        protected virtual void AutoCast() {
            bool hasHitNow = Cast();

            if (hasHitNow != hasHit)
            {
                if (hasHitNow) {
                    OnDetectionEnter?.Invoke();
                } else {
                    OnDetectionExit?.Invoke();
                }
            }

            hasHit = hasHitNow;
        }

        public virtual bool ManualCast()
        {
            hasHit = Cast();
            return hasHit;
        }

        protected abstract bool Cast();

        protected Vector3 CalculateCastPosition(float directionBodySize)
        {
            rotationOffset = transform.rotation * (offset + GlobalCastDirections[(int)direction] * (directionBodySize) * Convert.ToInt32(hasDirection));
            relativePosition = transform.position + rotationOffset;
            return relativePosition;
        }
    }

}

