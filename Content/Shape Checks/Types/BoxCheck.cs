/* 
Copyright (C) 2023 INF

This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using INFAttributes;

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


