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


