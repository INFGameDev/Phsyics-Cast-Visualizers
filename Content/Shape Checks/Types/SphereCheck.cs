using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsCastVisualizers
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Physics Cast Visualizers/Shape Checks/Sphere Check")]
    public class SphereCheck : ShapeCheckVisualizer
    {
        [SerializeField] protected float radius = 1;

        protected override bool Cast()
        {
            return Physics.CheckSphere(CalculateCastPosition(radius), radius, collidingLayers, EvaluateTriggerDetection());
        }

        void OnDrawGizmos() 
        {
            if(Application.isPlaying && visualize && visualizeOverride)
            {
                Gizmos.color = hasHit ? Color.red : Color.green; 
                Gizmos.DrawWireSphere(relativePosition, radius);
            }
            else if (!Application.isPlaying && visualizeOverride)
            {
                Gizmos.color = Color.green; 
                Gizmos.DrawWireSphere(transform.position, radius);            
            }

        }
    }
}


