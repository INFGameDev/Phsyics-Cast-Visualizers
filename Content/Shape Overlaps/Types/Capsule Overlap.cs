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
    [AddComponentMenu("Physics Cast Visualizer/Shape Overlaps/Capsule Overlap")]
    public class CapsuleOverlap : ShapeOverlapVisualizer
    {
        [BoxDivider("Capsule Overlap Properties")]
        [SerializeField] private float radius;
        [SerializeField, AlwaysExpanded] private CEndPointBasedCast properties;

        protected override bool Cast()
        {
            properties.DetermineEndPoints();
  
            casting = true;
            castTimeFrame = Time.frameCount;
            hitResult = default;

           initialHitResults = Physics.OverlapCapsule(properties.startPoint, properties.endPoint, radius, collidingLayers, GetTriggerInteraction());

            if (initialHitResults.Length > 0)
                return CheckTags();

            return false;
        }

        protected override void OnDrawGizmos()
        {
            if (!visualize)
                return;

            if (!casting) // don't deremine endpoint when casting cause it's already been determined when the cast method is called
                properties.DetermineEndPoints();

            var leProperties = properties.CalculateCapsuleTransformProperties(radius);

            Gizmos.color = GetDebugColor();
            Gizmos.DrawWireSphere(properties.startPoint, radius);
            Gizmos.DrawWireMesh(castMesh, leProperties.pos, leProperties.rot, new Vector3(radius * 2, leProperties.length, radius * 2)); 
            Gizmos.DrawWireSphere(properties.endPoint, radius);
            Gizmos.color = default;
        }

        protected override void OnValidate() {
            base.OnValidate();
            radius = radius.Clamp(0, Mathf.Infinity);
            hideCastDirectionField = true;
        }
    }

}

