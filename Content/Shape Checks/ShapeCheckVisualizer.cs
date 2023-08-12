using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace PhysCastVisualier
{
    public abstract class ShapeCheckVisualizer : CastVisualizer<bool>
    {
        [BoxDivider("Shape Check Properties")]
        [SerializeField] protected bool hasDirection;
        [SerializeField] protected Mesh castMesh;
        [SerializeField] protected Vector3 offset;

        protected override void AutoCast() => EventCheck(Cast());

        protected abstract bool Cast();
        protected abstract void OnDrawGizmos();

        public bool ManualCast()
        {
            EventCheck(Cast());
            return hasHit;
        }

        protected Vector3 CalculateCastPosition(float directionBodySize)
        {
            rotationOffset = transform.rotation * (offset + GlobalCastDirections[(int)direction] * (directionBodySize) * Convert.ToInt32(hasDirection));
            relativePosition = transform.position + rotationOffset;
            return relativePosition;
        }
        
        protected override void EventCheck(bool hasHitNow)
        {
            if (hasHitNow != hasHit)
            {
                if (hasHitNow) {
                    OnDetectionEnter?.Invoke(hasHitNow);
                    InvokeOnDetectionEnter_(hasHitNow);
                } else {
                    OnDetectionExit?.Invoke(hasHitNow);
                    InvokeOnDetectionExit_(hasHitNow);
                }
            }
 
            hasHit = hasHitNow;  
            hitResult = hasHitNow;
        }

        protected override void StateResultReset()
        {
            if (autoCast)
            {
                hasHit = false;
                casting = false;
                hitResult = default;
            }
            else // manually casted
            {
                // check if currently casting, if so don't reset the values and wait for the next frame
                if (Time.frameCount != castTimeFrame)
                {
                    hasHit = false;
                    casting = false;
                    hitResult = default;
                }
            }
        }
    }
}

