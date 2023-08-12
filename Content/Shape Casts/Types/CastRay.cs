// @INF

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysCastVisualier
{
    [AddComponentMenu("Physics Cast Visualizer/Shape Casts/Cast Ray")]
    public class CastRay : ShapeCastVisualizer
    {
        protected override bool Cast()
        {
            casting = true;
            castTimeFrame = Time.frameCount;
            bool hasHitNow = false;
            
            Vector3 castDirection = GetLocalCastDirection(direction);
            Vector3 posWOffset = transform.position + castDirection * directionOriginOffset;

            if (Physics.Raycast(posWOffset, castDirection, out hitResult, maxDistance, collidingLayers, GetTriggerInteraction()))
                hasHitNow = CheckTags();

            return hasHitNow;
        }

        public RaycastHit ManualCast(
            CastDirection direction, 
            float distance, 
            LayerMask targetLayers, 
            bool detectTriggers
        )
        {
            this.direction = direction;
            this.maxDistance = distance;
            this.collidingLayers = targetLayers;
            this.GetTriggerInteraction(detectTriggers);
            EventCheck(Cast());
            return hitResult;
        }

        protected override void OnDrawGizmos()
        {
            if (!visualize)
                return;

            Vector3 castDirection = GetLocalCastDirection(direction);
            Vector3 posWOffset_RayStart = transform.position + castDirection * directionOriginOffset;
            Vector3 vectorDir = castDirection * maxDistance;

            Debug.DrawRay(posWOffset_RayStart, vectorDir, GetDebugColor());
        }
    }
}

