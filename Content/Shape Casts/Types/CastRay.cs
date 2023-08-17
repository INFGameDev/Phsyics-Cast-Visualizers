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

