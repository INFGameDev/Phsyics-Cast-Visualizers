/* 
Copyright 2023 INF

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
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
        [SerializeField] protected Mesh castMesh;

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

