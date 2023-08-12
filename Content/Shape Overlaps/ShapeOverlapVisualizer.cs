using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PhysCastVisualier
{
    public abstract class ShapeOverlapVisualizer : CastVisualizer<Collider[]>
    {
        [BoxDivider("Shape Overlap Properties")]
        [SerializeField] protected bool hasDirection;
        [SerializeField] protected Mesh castMesh;
        [SerializeField] protected Vector3 offset;
        [SerializeField, DisplayOnly] protected int hitCount;
        [SerializeField, TagsSelection, Space(5)] protected string[] targetTags;
        [SerializeField, TextArea(10, 10)] private string hitsConsole;
        protected Collider[] initialHitResults;
        protected override void AutoCast() => EventCheck(Cast());
        protected abstract bool Cast();
        protected abstract void OnDrawGizmos();

        public Collider[] ManualCast()
        {
            EventCheck(Cast());
            return GetHit();
        }

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
            if (hasHitNow != hasHit)
            {
                if (hasHitNow)
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

            if (hitResult != null)
            {
                hitCount = hitResult.Length;
                for (int i = 0; i < hitResult.Length; i++)
                {
                    hitsConsole += "\n" +
                        $"Name: {hitResult[i].name}" +
                        "\n" +
                        $"Tag:    {hitResult[i].tag}" +
                        "\n" +
                        $"Layer: {LayerMask.LayerToName(hitResult[i].gameObject.layer)}" + "\n";
                }
            }
            else
            {
                hitCount = 0;
            }
        }

        protected Vector3 CalculateCastPosition(float directionBodySize)
        {
            rotationOffset = transform.rotation * (offset + GlobalCastDirections[(int)direction] * (directionBodySize) * Convert.ToInt32(hasDirection));
            relativePosition = transform.position + rotationOffset;
            return relativePosition;
        }

        protected override void StateResultReset()
        {
            if (autoCast)
            {
                hasHit = false;
                casting = false;
                hitCount = 0;
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

