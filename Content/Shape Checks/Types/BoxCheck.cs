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
using System;

namespace PhysCastVisualier
{
    [AddComponentMenu("Physics Cast Visualizer/Shape Checks/Box Check")]
    public class BoxCheck : ShapeCheckVisualizer
    {
        [BoxDivider("Box Check Properties")]
        [SerializeField, ShowChildrenOnly] private CBoxSize boxSize;
        [SerializeField, ShowChildrenOnly] private CCastOffset castOffset;

        protected override bool Cast()
        {
            casting = true;
            castTimeFrame = Time.frameCount;
            boxSize.SetDirectionAxisSize(direction);
            castOffset.CalculateCastPosition(boxSize.directionAxisSize / 2, direction, transform);
            return Physics.CheckBox(castOffset.relativePosition, boxSize.size / 2, transform.rotation, collidingLayers, GetTriggerInteraction());
        }

        public bool ManualCast(Vector2 newSize)
        {
            boxSize.size = newSize;
            EventCheck(Cast());
            return hitResult;
        }

        public void SetSize(Vector3 size) => this.boxSize.size = size;

        void OnValidate() {
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


