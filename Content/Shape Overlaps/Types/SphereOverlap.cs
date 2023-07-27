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
        [SerializeField] protected float radius = 1;
        [SerializeField] protected bool hasDirection;

        protected override void Cast()
        {
            detectedColliders.Clear();
            initiallyDetectedColliders = new Collider[detectionCountLimit];
            int initialDetectedCount = Physics.OverlapSphereNonAlloc(CalculateCastPosition(radius), radius, initiallyDetectedColliders, collidingLayers, GetTriggerInteraction() );

            int detectedColliderCount = 0;
            for (int i = 0; i < initialDetectedCount; i++)
            {
                Collider newCollider = initiallyDetectedColliders[i];
                for (int i2 = 0; i2 < compareTags.Length; i2++)
                {
                    if (newCollider.CompareTag(compareTags[i2])) {
                        detectedColliders.Add(newCollider);
                        detectedColliderCount++;
                        break;
                    }

                    if (detectedColliderCount >= detectionCountLimit)
                        break;
                }

                if (detectedColliderCount >= detectionCountLimit)
                    break;
            }
            
            hasHit = detectedColliders.Count != 0;
        }

        protected Vector3 CalculateCastPosition(float directionBodySize)
        {
            rotationOffset = transform.rotation * (offset + GlobalCastDirections[(int)direction] * (directionBodySize) * Convert.ToInt32(hasDirection));
            relativePosition = transform.position + rotationOffset;
            return relativePosition;
        }

        protected override void OnDrawGizmos() 
        {
            base.OnDrawGizmos();
            if (!visualize)
                return;

            Gizmos.color = hasHit ? Color.red : Color.green; 
            Gizmos.DrawWireSphere(relativePosition, radius);
        }
    }   
}
