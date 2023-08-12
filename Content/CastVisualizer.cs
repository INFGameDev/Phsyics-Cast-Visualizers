// @INF

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.Profiling;

namespace PhysCastVisualier
{
    public abstract class CastVisualizer<T> : MonoBehaviour
    {
        public enum CastDirection { Forward, Back, Right, Left, Up, Down }
        protected Vector3[] GlobalCastDirections = { Vector3.forward, Vector3.back, Vector3.right, Vector3.left, Vector3.up, Vector3.down };

        [BoxDivider("Visualizer Properties")]

        [SerializeField] protected bool visualize;
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
        /// raycast cast check (unreliable accuracy: used for visualization renders)
        /// </summary>
        [field: SerializeField, DisplayOnly] protected bool casting;
        [field: SerializeField, DisplayOnly] public bool hasHit { get; protected set; } = false;

        protected Vector3 rotationOffset;
        protected Vector3 relativePosition;
        protected T hitResult = default;
        protected int castTimeFrame;
        [SerializeField, HideInInspector] protected bool hideCastDirectionField;

        public void Visualize(bool b) => visualize = b;
        protected QueryTriggerInteraction GetTriggerInteraction() => detectTriggers ? QueryTriggerInteraction.Collide : UnityEngine.QueryTriggerInteraction.Ignore;
        protected QueryTriggerInteraction GetTriggerInteraction(bool doDetect) => doDetect ? QueryTriggerInteraction.Collide : UnityEngine.QueryTriggerInteraction.Ignore;
        protected abstract void AutoCast();

        protected virtual void Update()
        {
            StateResultReset();
            Profiler.BeginSample("Cast Visualizer");
            if (autoCast)
            {
                AutoCast();
            }
            Profiler.EndSample();
        }

        protected void InvokeOnDetectionEnter_(T value) => OnDetectionEnter_?.Invoke(value);
        protected void InvokeOnDetectionExit_(T value) => OnDetectionExit_?.Invoke(value);
        protected abstract void EventCheck(bool hasHitNow);
        protected Color GetDebugColor() => hasHit ? hasHitColor : castColor;
        public virtual void ToggleAutoCast(bool dasAuto) => autoCast = dasAuto;
        public virtual T GetHit() => hitResult;

        protected virtual void StateResultReset()
        {
            if (autoCast)
            {
                hasHit = false;
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
    }
}


