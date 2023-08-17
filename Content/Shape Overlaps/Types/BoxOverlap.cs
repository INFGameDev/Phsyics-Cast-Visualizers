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
using System;
using UnityEngine.Profiling;

namespace PhysCastVisualier
{
    [AddComponentMenu("Physics Cast Visualizer/Shape Overlaps/Box Overlap")]
    public class BoxOverlap : ShapeOverlapVisualizer
    {
        [BoxDivider("Box Overlap Properties")]
        [SerializeField, ShowChildrenOnly] private CBoxSize boxSize;
        private float directionAxisSize = 0;

        public void SetSize(Vector3 size) => boxSize.size = size;
        protected override bool Cast()
        {
            casting = true;
            castTimeFrame = Time.frameCount;
            hitResult = default;

            boxSize.SetDirectionAxisSize(direction);
            castOffset.CalculateCastPosition(boxSize.directionAxisSize / 2, direction, transform);
            initialHitResults = Physics.OverlapBox(castOffset.relativePosition, boxSize.size / 2, transform.rotation, collidingLayers, GetTriggerInteraction());

            if (initialHitResults.Length > 0)
                return CheckTags();

            return false;
        }

        public Collider[] ManualCast(Vector3 newSize)
        {
            boxSize.size = newSize;
            EventCheck(Cast());
            return hitResult;
        }

        protected override void OnValidate() {
            base.OnValidate();

            if (boxSize != null)
                boxSize.PositiveInfinityClamp();
        }

        protected override void OnDrawGizmos()
        {
            if (!visualize)
                return;

            if (!casting)
            {
                boxSize.SetDirectionAxisSize(direction);
                castOffset.CalculateCastPosition(boxSize.directionAxisSize / 2, direction, transform);
            }

            Gizmos.color = GetDebugColor();
            Gizmos.DrawWireMesh(castMesh, castOffset.relativePosition, transform.rotation, boxSize.size);
        }
    }
}

