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


