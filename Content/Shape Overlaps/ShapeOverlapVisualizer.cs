/* 
Copyright (C) 2023 INF

This code is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License as published
by the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PhysCastVisualier
{
    public abstract class ShapeOverlapVisualizer : CastVisualizer<Collider[]>
    {
        [BoxDivider("Shape Overlap Properties")]
        [SerializeField] protected Mesh castMesh;
        [SerializeField, ShowChildrenOnly] protected CCastOffset castOffset;
        [SerializeField, DisplayOnly] protected int hitCount;
        [SerializeField, TagsSelection, Space(5)] protected string[] targetTags;
        [SerializeField, TextArea(10, 10)] private string hitsConsole;
        protected Collider[] initialHitResults;
        protected override void AutoCast() => EventCheck(Cast());
        protected abstract bool Cast();
        protected abstract void OnDrawGizmos();
        protected Vector3 rotationOffset;
        protected Vector3 relativePosition;

        public Collider[] ManualCast()
        {
            EventCheck(Cast());
            return GetHit();
        }

        #region User Accessed Methods =====================================================================

            // Cast Properties Setter Methods ================================================================
            public void SetTargetTags(string[] tags) => targetTags = tags;
            public void SetOffset(Vector3 offset) => castOffset.offset = offset;
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

            // Cast Property Getter Methods ==================================================================

        #endregion User Accessed Methods =====================================================================

        public override Collider[] GetHit()
        {
            if (hasHit && hitResult != null)
                return base.GetHit();

            Debug.LogWarning("There are no hit results, returning an empty collider array instead!");
            return new Collider[]{};
        }

        protected bool CheckTags()
        {
            bool taggedHitFound = false;
            List<Collider> collidersWithTargetTags = new List<Collider>();

            // check if there is any hits
            if (initialHitResults.Length > 0)
            {
                // loop through all initial jits
                for (int i = 0; i < initialHitResults.Length; i++)
                {
                    // check if there is any set target tags
                    if (targetTags.Length > 0)
                    {
                        // loop through all target tags
                        for (int j = 0; j < targetTags.Length; j++)
                        {
                            if (!String.IsNullOrEmpty(targetTags[j]) && initialHitResults[i].CompareTag(targetTags[j]))
                            {
                                collidersWithTargetTags.Add(initialHitResults[i]);
                                taggedHitFound = true;
                            }
                        }
                    }
                    else // if there isn't, just validate all hits as accepted
                    {
                        collidersWithTargetTags.AddRange(initialHitResults);
                        taggedHitFound = true;
                        break;
                    }
                }
            }

            if (collidersWithTargetTags.Count > 0)
                hitResult = collidersWithTargetTags.ToArray();

            return taggedHitFound;
        }

        protected virtual void OnValidate() 
        {
            if (!Application.isPlaying)
                hitsConsole = String.Empty;
        }

        protected override void EventCheck(bool hasHitNow)
        {
            int hitLength = hitResult != null ? hitResult.Length : 0;

            if (hitLength != hitCount) {
                if (hitLength > hitCount) 
                {
                    OnDetectionEnter?.Invoke(hitResult);
                    InvokeOnDetectionEnter_(hitResult);
                }
                else
                {
                    OnDetectionExit?.Invoke(hitResult);
                    InvokeOnDetectionExit_(hitResult);
                }
            }
                
            hasHit = hasHitNow;
            hitsConsole = String.Empty;

            for (int i = 0; i < hitLength; i++)
            {
                hitsConsole += "\n" +
                    $"Name: {hitResult[i].name}" +
                    "\n" +
                    $"Tag:    {hitResult[i].tag}" +
                    "\n" +
                    $"Layer: {LayerMask.LayerToName(hitResult[i].gameObject.layer)}" + "\n";
            }

            hitCount = hitLength;

        }

        protected override void StateResultReset()
        {
            if (autoCast)
            {
                // hasHit = false;
                casting = false;
                // hitCount = 0;
                hitsConsole = String.Empty;
                hitResult = default;
                initialHitResults = default;
            }
            else // manually casted
            {
                // check if currently casting, if so don't reset the values and wait for the next frame
                if (Time.frameCount != castTimeFrame)
                {
                    hasHit = false;
                    casting = false;
                    hitCount = 0;
                    hitsConsole = String.Empty;
                    hitResult = default;
                    initialHitResults = default;
                }
            }
        }
    }
}

