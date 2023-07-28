using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysCastVisualier
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Physics Cast Visualizer/Shape Checks/Sphere Check")]
    public class SphereCheck : ShapeCheckVisualizer
    {
        [SerializeField] protected float radius = 1;

        protected override bool Cast() => Physics.CheckSphere(CalculateCastPosition(radius), radius, collidingLayers, GetTriggerInteraction());
        protected void OnDrawGizmos() 
        {
            if (!visualize)
                return;

            // the forbidden disgusting nested ternary
            Gizmos.color = Application.isPlaying ? (hasHit ? Color.red : Color.green) : Color.white; 
            Gizmos.DrawWireSphere(transform.position, radius);  
        }
    }
}


