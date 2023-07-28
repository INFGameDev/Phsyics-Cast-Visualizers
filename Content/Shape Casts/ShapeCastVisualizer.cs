using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace PhysCastVisualier
{
    public abstract class ShapeCastVisualizer : CastVisualizer
    {
        public event Action<RaycastHit> OnDetectionEnter_;
        public event Action<RaycastHit> OnDetectionExit_;
        protected RaycastHit _hit;


        [BoxDivider("Shape Cast Properties")]

        [SerializeField] protected float maxDistance;
        
        [SerializeField, TagsSelection, Space(5)] protected string[] targetTags;
        [SerializeField, Space(5)] protected float directionOriginOffset;
        [SerializeField, Space(5)] protected UnityEvent<RaycastHit> OnDetectionEnter;
        [SerializeField, Space(5)] protected UnityEvent<RaycastHit> OnDetectionExit;

        [SerializeField, DisplayOnly, Space(5)] protected string hitName;
        [SerializeField, DisplayOnly, Space(5)] protected string layerhit;
        [SerializeField, DisplayOnly, Space(5)] protected string hitTag;

        
        public RaycastHit? hit {
            get 
                {
                    if (_hit.collider != null && hasHit)
                        return _hit;

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
            directionOriginOffset = Mathf.Clamp(directionOriginOffset, 0, Mathf.Infinity);
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void AutoCast()
        {
            EventCheck(Cast());
        }
        protected abstract bool Cast();
        protected bool CheckTags()
        {
            bool taggedHitFound = false;
            if (targetTags.Length > 0) {
                for (int i = 0; i < targetTags.Length; i++) {
                    if (_hit.transform.CompareTag(targetTags[i])) {
                        taggedHitFound = true;
                        break;
                    }
                }                     
            } else {
                taggedHitFound = true;                   
            }

            return taggedHitFound;
        }

        protected void EventCheck(bool hasHitNow)
        {
            if (hasHitNow != hasHit)
            {
                if (hasHitNow) {
                    OnDetectionEnter?.Invoke(_hit);
                    OnDetectionEnter_?.Invoke(_hit);
                } else {
                    OnDetectionExit?.Invoke(_hit);
                    OnDetectionExit_?.Invoke(_hit);
                }
            }

            hasHit = hasHitNow;

            if (_hit.collider != null) {
                hitName =_hit.collider.name;
                hitTag = _hit.collider.tag;
                layerhit = LayerMask.LayerToName(_hit.collider.gameObject.layer);
            }
            else {
                hitName = string.Empty;
                hitTag = string.Empty;
                layerhit = default;
            }   
        }
    }

}

