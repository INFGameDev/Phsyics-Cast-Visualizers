/* 
Copyright (C) 2023 INF

This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.Profiling;
using INFAttributes;

namespace PhysCastVisualier
{
    public enum CastDirection { None, Forward, Back, Right, Left, Up, Down }

    public abstract class CastVisualizer<T> : MonoBehaviour
    {
        public static readonly Vector3[] GlobalCastDirections = { Vector3.zero, Vector3.forward, Vector3.back, Vector3.right, Vector3.left, Vector3.up, Vector3.down };

        [BoxDivider("Visualizer Properties")]
        [SerializeField] protected bool visualize;
        [SerializeField, DisplayIf(nameof(hideSelfCastField), true)] private bool selfCast = true;
        [SerializeField, HideInInspector] protected bool hideSelfCastField;
        [SerializeField, DisplayIf(nameof(selfCast), true)] private Transform referenceTransform;
        protected Transform castTransform => selfCast ? transform : referenceTransform;
        [SerializeField] protected LayerMask collidingLayers;
        [SerializeField, DisplayIf(nameof(hideCastDirectionField), true)] protected CastDirection direction;
        [SerializeField] protected bool detectTriggers;
        [SerializeField] protected bool autoCast = true;
        [SerializeField] protected Color castColor = Color.white;
        [SerializeField] protected Color hasHitColor = Color.red;

        [SerializeField, Space(5)] protected UnityEvent<T> OnDetectionEnter;
        [SerializeField, Space(5)] protected UnityEvent<T> OnDetectionExit;
        public event Action<T> OnDetectionEnter_;
        public event Action<T> OnDetectionExit_;

        /// <summary>
        /// returns if a cast is happening in the current frame (get's reset back to false next frame if cast is not active)
        /// </summary>
        [field: SerializeField, DisplayOnly] protected bool casting;
        [field: SerializeField, DisplayOnly] public bool hasHit { get; protected set; } = false;
        protected T hitResult = default;


        /// <summary>
        /// stores the frame the last known cast is made
        /// </summary>
        protected int castTimeFrame;
        [SerializeField, HideInInspector] protected bool hideCastDirectionField;


        protected QueryTriggerInteraction GetTriggerInteraction() => detectTriggers ? QueryTriggerInteraction.Collide : UnityEngine.QueryTriggerInteraction.Ignore;
        protected QueryTriggerInteraction GetTriggerInteraction(bool doDetect) => doDetect ? QueryTriggerInteraction.Collide : UnityEngine.QueryTriggerInteraction.Ignore;
        protected void InvokeOnDetectionEnter_(T value) => OnDetectionEnter_?.Invoke(value);
        protected void InvokeOnDetectionExit_(T value) => OnDetectionExit_?.Invoke(value);
        protected Color GetDebugColor() => hasHit ? hasHitColor : castColor;        
        #region Overridable Methods ==============================================================================================
            protected abstract void AutoCast();
            protected abstract void EventCheck(bool hasHitNow);
            protected virtual void Update()
            {
                StateResultReset();
                // Profiler.BeginSample("Cast Visualizer");

                if (autoCast)
                    AutoCast();

                // Profiler.EndSample();
            }

            protected virtual void StateResultReset()
            {
                if (autoCast)
                {
                    hasHit = false;
                    casting = false;
                }
                else // manually casted
                {
                    // check if currently casting, if so don't reset the values and wait for the next frame
                    if (Time.frameCount != castTimeFrame)
                    {
                        casting = false;
                        hasHit = false;
                    }
                }
            }
        #endregion Overridable Methods ==============================================================================================

        #region User Accessed Methods =====================================================================

            // Cast Properties Setter Methods ================================================================
            public void Visualize(bool b) => visualize = b;
            public virtual void ToggleAutoCast(bool dasAuto) => autoCast = dasAuto;
            public void SetCollidingLayers(LayerMask layerMask) => collidingLayers = layerMask;
            public void SetCastingDirection(CastDirection direction) => this.direction = direction;
            public void SetDetectTriggers(bool doDetect) => detectTriggers = doDetect;

            // Cast Property Getter Methods ==================================================================
            public virtual T GetHit() => hitResult;

        #endregion User Accessed Methods =====================================================================
    }
}


