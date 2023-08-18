/* 
Copyright (C) 2023 INF

This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
file, You can obtain one at http://mozilla.org/MPL/2.0/.
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

