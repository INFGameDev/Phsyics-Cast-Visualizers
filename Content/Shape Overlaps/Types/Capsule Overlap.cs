/* 
Copyright (C) 2023 INF

This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using INFAttributes;

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

            if (overlapCastType == OverlapCastType.Normal){
                initialHitResults = Physics.OverlapCapsule(properties.startPoint, properties.endPoint, radius, collidingLayers, GetTriggerInteraction());

                if (initialHitResults.Length > 0)
                    return CheckTags();
            } else {
                initialHitResults = new Collider[allocSize];
                Physics.OverlapCapsuleNonAlloc(properties.startPoint, properties.endPoint, radius, initialHitResults, collidingLayers, GetTriggerInteraction());

                if (initialHitResults[0] != null)
                    return CheckTags();
            }

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
            hideSelfCastField = true;
        }
    }

}

