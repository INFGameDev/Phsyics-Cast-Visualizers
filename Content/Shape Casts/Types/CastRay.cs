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
    [AddComponentMenu("Physics Cast Visualizer/Shape Casts/Cast Ray")]
    public class CastRay : ShapeCastVisualizer
    {
        protected override bool Cast()
        {
            casting = true;
            castTimeFrame = Time.frameCount;
            CalculateDirAndPos();

            if (Physics.Raycast(posWOffset, castDirection, out hitResult, maxDistance, collidingLayers, GetTriggerInteraction()))
                return CheckTags();

            return false;
        }

        public RaycastHit ManualCast( CastDirection newDireaction, float newDistance = -1 )
        {
            this.direction = newDireaction;
            this.maxDistance = newDistance == -1 ? this.maxDistance : newDistance;
            EventCheck(Cast());
            return hitResult;
        }

        protected override void OnDrawGizmos()
        {
            if (!visualize)
                return;

            if (!casting)
                CalculateDirAndPos();

            Vector3 vectorDir = castDirection * maxDistance;
            Debug.DrawRay(posWOffset, vectorDir, GetDebugColor());
        }
    }
}

