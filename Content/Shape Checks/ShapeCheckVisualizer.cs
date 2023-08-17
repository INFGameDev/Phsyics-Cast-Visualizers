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

