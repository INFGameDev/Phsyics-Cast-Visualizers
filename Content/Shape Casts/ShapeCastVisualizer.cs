/* 
Copyright (C) 2023 INF

This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

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
        [SerializeField, DisplayOnly, Space(5)] protected string layerHit;
        [SerializeField, DisplayOnly, Space(5)] protected string hitTag;
        [SerializeField, HideInInspector] protected bool hideMaxDistanceField;
        [SerializeField, HideInInspector] protected bool hideDirectionOriginOffsetField;
        protected Vector3 castDirection;
        protected Vector3 posWOffset;



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

            if (!taggedHitFound)
                hitResult = default;

            return taggedHitFound;
        }

        #region User Accessed Methods =====================================================================

            // Cast Properties Setter Methods ================================================================
            public void SetMaxDistance(float distance) => maxDistance = distance;
            public void SetTargetTags(string[] tags) => targetTags = tags;
            public void AddTag(string tag)
            {
                string[] oldTags = targetTags; // store old as temp
                string[] newTags = new string[oldTags.Length + 1];

                // loop through longer new tags array to add the old tags and the new one
                for (int i = 0; i < newTags.Length; i++)
                {
                    // check if we are in the end of the array
                    if (i == newTags.Length - 1){
                        newTags[i] = tag; // add the new tag
                    } else {
                        newTags[i] = oldTags[i]; // copy the old one
                    }
                }

                // assign the new tags
                targetTags = newTags;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="tag"></param>
            /// <returns>Is Removal Sucessful</returns>
            public bool RemoveTag(string tag)
            {
                // ===========================================================================================
                // Check if the array contains the tag being removed

                bool tagFound = false;
                for (int i = 0; i < targetTags.Length; i++)
                {
                    if (targetTags[i] == tag) 
                        tagFound = true;
                }

                if (!tagFound){
                    Debug.LogWarning($"The tag being removed ({tag}) is non-existent in the Target Tags");
                    return false;
                }

                // ===========================================================================================
                    
                string[] oldTags = targetTags;
                string[] newTags = new string[oldTags.Length - 1];

                int newTagsIndexCount = 0;
                for (int i = 0; i < oldTags.Length; i++)
                {
                    // if the old tag doesn't match the tag being removed add it to the new tag array
                    // the matching old tag will instead be ignored
                    if (oldTags[i] != tag){
                        newTags[newTagsIndexCount] = oldTags[i];
                        newTagsIndexCount++;
                    }
                }

                targetTags = newTags;
                return true;
            }

            public void SetDirectionOriginOffset(float offset) => directionOriginOffset = offset;

            // Cast Property Getter Methods ==================================================================

        #endregion User Accessed Methods =====================================================================

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

            if (hitResult.collider != null && hasHit) {
                hitName = hitResult.collider.name;
                hitTag = hitResult.collider.tag;
                layerHit = LayerMask.LayerToName(hitResult.collider.gameObject.layer);
            }
            else {
                hitName = string.Empty;
                hitTag = string.Empty;
                layerHit = default;
            }   
        }

        protected override void StateResultReset()
        {
            if (autoCast)
            {
                // hasHit = false;
                casting = false;
                hitName = String.Empty;
                hitTag = String.Empty;
                layerHit = String.Empty;
                hitResult = default;
            }
            else // manually casted
            {
                // check if currently casting, if so don't reset the values and wait for the next frame
                if (Time.frameCount != castTimeFrame)
                {
                    hasHit = false;
                    casting = false;
                    hitName = String.Empty;
                    hitTag = String.Empty;
                    layerHit = String.Empty;
                    hitResult = default;
                }
            }
        }

        protected void CalculateDirAndPos()
        {
            castDirection = GetLocalCastDirection(direction);
            posWOffset = castTransform.position + castDirection * directionOriginOffset;
        }

        protected Vector3 GetLocalCastDirection(CastDirection direction)
        {
            Vector3 localDirection = Vector3.zero;

            switch (direction)
            {
                case CastDirection.Right:
                    localDirection = castTransform.right;
                    break;
                case CastDirection.Left:
                    localDirection = -castTransform.right;
                    break;
                case CastDirection.Up:
                    localDirection = castTransform.up;
                    break;
                case CastDirection.Down:
                    localDirection = -castTransform.up;
                    break;
                case CastDirection.Forward:
                    localDirection = castTransform.forward;
                    break;
                case CastDirection.Back:
                    localDirection = -castTransform.forward;
                    break;
                default:
                    localDirection = Vector3.zero;
                    break;
            }  

            return localDirection;
        }
    }

}

