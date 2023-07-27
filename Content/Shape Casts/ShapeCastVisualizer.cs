using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace PhysCastVisualier
{
    public abstract class ShapeCastVisualizer : CastVisualizer<RaycastHit?>
    {
        public event Action<RaycastHit> OnDetectionEnter_;
        public event Action<RaycastHit> OnDetectionExit_;
        protected RaycastHit hit_;


        [BoxDivider("Shape Cast Properties")]

        [SerializeField] protected float maxDistance;
        
        [SerializeField, TagsSelection, Space(5)] protected string[] targetTags;
        [SerializeField, Space(5)] protected float directionOriginOffset;
        [SerializeField, Space(5)] protected UnityEvent<RaycastHit> OnDetectionEnter;
        [SerializeField, Space(5)] protected UnityEvent<RaycastHit> OnDetectionExit;

        [SerializeField, DisplayOnly, Space(5)] protected string hitName;

        
        public RaycastHit? hit {
            get 
                {
                    if (hit_.collider != null && hasHit)
                        return hit_;

                    return null;
                }
        }

        protected Vector3 GetLocalCastDirection(CastDirection direction)
        {

            Vector3 castDirection = Vector3.zero;

            switch (direction)
            {
                case CastDirection.Right:
                    castDirection = transform.right;
                    break;
                case CastDirection.Left:
                    castDirection = -transform.right;
                    break;
                case CastDirection.Up:
                    castDirection = transform.up;
                    break;
                case CastDirection.Down:
                    castDirection = -transform.up;
                    break;
                case CastDirection.Forward:
                    castDirection = transform.forward;
                    break;
                case CastDirection.Back:
                    castDirection = -transform.forward;
                    break;
            }   

            return castDirection;
        }

        protected virtual void OnValidate() {
            maxDistance = Mathf.Clamp(maxDistance, 0, Mathf.Infinity);
            // directionOriginOffset = Mathf.Clamp(directionOriginOffset, 0, Mathf.Infinity);
        }

        protected override void AutoCast()
        {
            bool hasHitNow = Cast();

            if (hasHitNow != hasHit)
            {
                if (hasHitNow) {
                    OnDetectionEnter?.Invoke(hit_);
                    OnDetectionEnter_?.Invoke(hit_);
                } else {
                    OnDetectionExit?.Invoke(hit_);
                    OnDetectionExit_?.Invoke(hit_);
                }
            }

            hasHit = hasHitNow;
            hitName = hit_.collider != null ? hit_.collider.name : string.Empty;
        }

        protected abstract bool Cast();

        public override RaycastHit? ManualCast()
        {
            autoCast = false;
            hasHit =  Cast();
            return hit;
        }
    }

}

