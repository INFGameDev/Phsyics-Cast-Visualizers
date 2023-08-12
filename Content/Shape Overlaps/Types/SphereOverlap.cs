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

        protected override bool Cast()
        {
            casting = true;
            castTimeFrame = Time.frameCount;
            hitResult = default;
            initialHitResults = Physics.OverlapSphere(CalculateCastPosition(radius), radius, collidingLayers, GetTriggerInteraction());

            if (initialHitResults.Length > 0)
            {
                return CheckTags();
            }

            return false;
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

            Gizmos.color = GetDebugColor();
            Gizmos.DrawWireMesh(castMesh,  CalculateCastPosition(radius), transform.rotation, Vector3.one * radius * 2);
        }
    }   
}
