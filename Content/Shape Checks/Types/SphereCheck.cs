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
    [AddComponentMenu("Physics Cast Visualizer/Shape Checks/Sphere Check")]
    public class SphereCheck : ShapeCheckVisualizer
    {
        [BoxDivider("Sphere Check Properties")]
        [SerializeField] protected float radius = 1;
        [SerializeField, ShowChildrenOnly] private CCastOffset castOffset;
        public void SetRadius(float radius) =>  this.radius = radius;

        protected override bool Cast()
        {
            castTimeFrame = Time.frameCount;
            casting = true;
            castOffset.CalculateCastPosition(radius, direction, transform);
            return Physics.CheckSphere(castOffset.relativePosition, radius, collidingLayers, GetTriggerInteraction());
        }

        public bool ManualCast(float newRadius)
        {
            radius = newRadius;
            EventCheck(Cast());
            return hitResult;
        }

        protected override void OnDrawGizmos()
        {
            if (!visualize)
                return;

            if (!casting)
                castOffset.CalculateCastPosition(radius, direction, transform);

            Gizmos.color = GetDebugColor();
            Gizmos.DrawWireMesh(castMesh, castOffset.relativePosition, transform.rotation, Vector3.one * radius * 2);
        }

        void OnValidate() {
            radius = radius.Clamp(0, Mathf.Infinity);
        }
    }
}


