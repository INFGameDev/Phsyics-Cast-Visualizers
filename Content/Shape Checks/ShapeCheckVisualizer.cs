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

namespace PhysCastVisualier
{
    public abstract class ShapeCheckVisualizer : CastVisualizer<bool>
    {
        [BoxDivider("Shape Check Properties")]
        [SerializeField, DisplayOnly] protected Mesh castMesh;

        protected override void AutoCast() => EventCheck(Cast());

        protected abstract bool Cast();
        protected abstract void OnDrawGizmos();

        public bool ManualCast()
        {
            EventCheck(Cast());
            return hasHit;
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
                // hasHit = false;
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

