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

