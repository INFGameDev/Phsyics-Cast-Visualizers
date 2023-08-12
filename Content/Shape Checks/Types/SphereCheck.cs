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

        protected override bool Cast()
        {
            castTimeFrame = Time.frameCount;
            casting = true;
            return Physics.CheckSphere(CalculateCastPosition(radius), radius, collidingLayers, GetTriggerInteraction());
        }
        protected override void OnDrawGizmos()
        {
            if (!visualize)
                return;

            Gizmos.color = GetDebugColor();
            Gizmos.DrawWireMesh(castMesh,  CalculateCastPosition(radius), transform.rotation, Vector3.one * radius * 2);
        }

        void OnValidate() {
            radius = radius.Clamp(0, Mathf.Infinity);
        }
    }
}


