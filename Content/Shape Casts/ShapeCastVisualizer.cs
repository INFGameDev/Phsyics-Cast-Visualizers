using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace PhysCastVisualier
{
    public abstract class ShapeCastVisualizer : CastVisualizer<RaycastHit>
    {
        [BoxDivider("Shape Cast Properties")]
        [SerializeField, DisplayIf(nameof(hideMaxDistanceField), true)] protected float maxDistance;
        [SerializeField, TagsSelection, Space(5)] protected string[] targetTags;
        [SerializeField, DisplayIf(nameof(hideDirectionOriginOffsetField), true),Space(5)] protected float directionOriginOffset;
        [SerializeField, DisplayOnly, Space(5)] protected string hitName;
        [SerializeField, DisplayOnly, Space(5)] protected string layerhit;
        [SerializeField, DisplayOnly, Space(5)] protected string hitTag;
        [SerializeField, HideInInspector] protected bool hideMaxDistanceField;
        [SerializeField, HideInInspector] protected bool hideDirectionOriginOffsetField;

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

        protected override void AutoCast() => EventCheck(Cast());
        protected abstract bool Cast();
        protected abstract void OnDrawGizmos();
        protected bool CheckTags()
        {
            bool taggedHitFound = false;
            if (targetTags.Length > 0) {
                for (int i = 0; i < targetTags.Length; i++) {
                    if (!String.IsNullOrEmpty(targetTags[i]) && hitResult.transform.CompareTag(targetTags[i])) {
                        taggedHitFound = true;
                        break;
                    }
                }                     
            } else {
                taggedHitFound = true;                   
            }

            return taggedHitFound;
        }

        public RaycastHit ManualCast()
        {
            EventCheck(Cast());
            return hitResult;
        }

        protected override void EventCheck(bool hasHitNow)
        {
            if (hasHitNow != hasHit)
            {
                if (hasHitNow) {
                    OnDetectionEnter?.Invoke(hitResult);
                    InvokeOnDetectionEnter_(hitResult);
                } else {
                    OnDetectionExit?.Invoke(hitResult);
                    InvokeOnDetectionExit_(hitResult);
                }
            }

            hasHit = hasHitNow;

            if (hitResult.collider != null) {
                hitName =hitResult.collider.name;
                hitTag = hitResult.collider.tag;
                layerhit = LayerMask.LayerToName(hitResult.collider.gameObject.layer);
            }
            else {
                hitName = string.Empty;
                hitTag = string.Empty;
                layerhit = default;
            }   
        }

        protected override void StateResultReset()
        {
            if (autoCast)
            {
                hasHit = false;
                casting = false;
                hitName = String.Empty;
                hitTag = String.Empty;
                layerhit = String.Empty;
                hitResult = default;
            }
            else // manually casted
            {
                // check if currently casting, if so don't reset the values and wait for the next frame
                if (Time.frameCount != castTimeFrame)
                {
                    casting = false;
                    hasHit = false;
                    hitName = String.Empty;
                    hitTag = String.Empty;
                    layerhit = String.Empty;
                    hitResult = default;
                }
            }
        }
    }

}

