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
    [AddComponentMenu("Physics Cast Visualizer/Shape Overlaps/Sphere Overlap")]
    public class SphereOverlap : ShapeOverlapVisualizer
    {
        [BoxDivider("Sphere Overlap Properties")]
        [SerializeField] protected float radius = 1;

        public void SetRadius(float radius) => this.radius = radius;
        protected override bool Cast()
        {
            casting = true;
            castTimeFrame = Time.frameCount;
            hitResult = default;

            castOffset.CalculateCastPosition(radius, direction, transform);
            initialHitResults = Physics.OverlapSphere(castOffset.relativePosition, radius, collidingLayers, GetTriggerInteraction());

            if (initialHitResults.Length > 0)
                return CheckTags();
                
            return false;
        }

        public Collider[] ManualCast(float newRadius)
        {
            radius = newRadius;
            EventCheck(Cast());
            return hitResult;
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            radius = radius.Clamp(0, Mathf.Infinity);
        }

        protected override void OnDrawGizmos()
        {
            if (!visualize)
                return;

            if (!casting)
                castOffset.CalculateCastPosition(radius, direction, transform);

            Gizmos.color = GetDebugColor();
            Gizmos.DrawWireMesh(castMesh,  castOffset.relativePosition, transform.rotation, Vector3.one * radius * 2);
        }
    }   
}
