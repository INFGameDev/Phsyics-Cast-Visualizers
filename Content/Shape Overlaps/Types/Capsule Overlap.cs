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

