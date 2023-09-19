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
using UnityEngine.Profiling;
using INFAttributes;

namespace PhysCastVisualier
{
    [AddComponentMenu("Physics Cast Visualizer/Shape Overlaps/Sphere Overlap")]
    public class SphereOverlap : ShapeOverlapVisualizer
    {
        [BoxDivider("Sphere Overlap Properties")]
        [SerializeField] protected float radius = 1;

        public void SetRadius(float radius) => this.radius = radius;
        protected override bool Cast()
        {
            casting = true;
            castTimeFrame = Time.frameCount;
            hitResult = default;

            castOffset.CalculateCastPosition(radius, direction, castTransform);

            if (overlapCastType == OverlapCastType.Normal){
                initialHitResults = Physics.OverlapSphere(castOffset.relativePosition, radius, collidingLayers, GetTriggerInteraction());

                if (initialHitResults.Length > 0)
                    return CheckTags();
            } else {
                initialHitResults = new Collider[allocSize];
                Physics.OverlapSphereNonAlloc(castOffset.relativePosition, radius, initialHitResults, collidingLayers, GetTriggerInteraction());

                if (initialHitResults[0] != null)
                    return CheckTags();
            }
                
            return false;
        }

        public Collider[] ManualCast(float newRadius)
        {
            radius = newRadius;
            EventCheck(Cast());
            return hitResult;
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            radius = radius.Clamp(0, Mathf.Infinity);
        }

        protected override void OnDrawGizmos()
        {
            if (!visualize)
                return;

            if (!casting)
                castOffset.CalculateCastPosition(radius, direction, castTransform);

            Gizmos.color = GetDebugColor();
            Gizmos.DrawWireSphere(castOffset.relativePosition, radius);
        }
    }   
}
