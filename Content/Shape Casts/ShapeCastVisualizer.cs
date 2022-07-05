using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using MyBox;

namespace PhysicsCastVisualizers
{
    public abstract class ShapeCastVisualizer : CastVisualizer
    {
        [SerializeField] protected float maxDistance;
        [SerializeField] protected float directionOriginOffset;
        [SerializeField, Tag] protected string[] targetTags;
        protected float directionAxisSize = 0;
        protected RaycastHit hit;
        protected Vector3 GetLocalCastDirection(CastDirection direction)
        {
            Vector3 castDirection = Vector3.zero;

            switch (direction)
            {
                case CastDirection.Right:
                    castDirection = transform.right;
                    break;
                case CastDirection.Left:
                    castDirection = -transform.right;
                    break;
                case CastDirection.Up:
                    castDirection = transform.up;
                    break;
                case CastDirection.Down:
                    castDirection = -transform.up;
                    break;
                case CastDirection.Forward:
                    castDirection = transform.forward;
                    break;
                case CastDirection.Back:
                    castDirection = -transform.forward;
                    break;
            }   

            return castDirection;
        }

        protected virtual void OnValidate() {
            maxDistance = Mathf.Clamp(maxDistance, 0, Mathf.Infinity);
            directionOriginOffset = Mathf.Clamp(directionOriginOffset, 0, Mathf.Infinity);
        }

        protected void Update() 
        {
            if (autoCast)
                Cast();
        }

        protected abstract void Cast();
        public virtual RaycastHit? ManualCast()
        {
            autoCast = false;
            Cast();

            if (hasHit)
                return hit;

            return null;
        }
    }

}

