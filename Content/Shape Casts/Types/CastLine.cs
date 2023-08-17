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

namespace PhysCastVisualier
{
    [AddComponentMenu("Physics Cast Visualizer/Shape Casts/Cast Line")]
    public class CastLine : ShapeCastVisualizer
    {
        [BoxDivider("Cast Line Properties")]
        [SerializeField, AlwaysExpanded] private CEndPointBasedCast properties;

        protected override bool Cast()
        {
            properties.DetermineEndPoints();
            casting = true; 
            castTimeFrame = Time.frameCount;

            if (Physics.Linecast(properties.startPoint, properties.endPoint, out hitResult, collidingLayers, GetTriggerInteraction()))
                return CheckTags();

            return false;
        }

        protected override void OnValidate() {
            base.OnValidate();
            hideCastDirectionField = true;
            hideMaxDistanceField = true;
            hideDirectionOriginOffsetField = true;
        }

        protected override void OnDrawGizmos()
        {
            if (!visualize)
                return;

            if (!casting) // don't deremine endpoint when casting cause it's already been determined when the cast method is called
                properties.DetermineEndPoints();

            Gizmos.color = GetDebugColor();
            Gizmos.DrawWireSphere(properties.startPoint, 0.05f);
            Gizmos.DrawWireSphere(properties.endPoint, 0.05f);
            Gizmos.DrawLine(properties.startPoint, properties.endPoint);
            Gizmos.color = default;
        }
    }
}

