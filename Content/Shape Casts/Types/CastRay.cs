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

