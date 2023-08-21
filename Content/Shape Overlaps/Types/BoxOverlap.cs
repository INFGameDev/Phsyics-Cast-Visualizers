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
using UnityEngine.Profiling;

namespace PhysCastVisualier
{
    [AddComponentMenu("Physics Cast Visualizer/Shape Overlaps/Box Overlap")]
    public class BoxOverlap : ShapeOverlapVisualizer
    {
        [BoxDivider("Box Overlap Properties")]
        [SerializeField, ShowChildrenOnly] private CBoxSize boxSize;

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

