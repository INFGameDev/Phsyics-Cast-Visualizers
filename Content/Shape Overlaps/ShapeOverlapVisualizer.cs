using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

namespace PhysicsCastVisualizers
{
    public abstract class ShapeOverlapVisualizer : CastVisualizer
    {
        [SerializeField] protected int detectionCountLimit = 1;
        [SerializeField, DisplayOnly] protected List<Collider> detectedColliders;
        [SerializeField, Tag] protected string[] compareTags;
        [SerializeField] protected Mesh castMesh;
        [SerializeField] protected Vector3 offset;
        protected Collider[] initiallyDetectedColliders;
        protected Collider[] finalOutput;
        protected virtual void Update() 
        {
            rotation = useParentRot ? transform.parent.rotation : transform.rotation;

            if (autoCast)
                AutoCast();
        }

        protected virtual void AutoCast() => Cast();

        public virtual bool ManualCast()
        {
            autoCast = false;
            Cast();
            return hasHit;
        }

        protected abstract void Cast(); 
    }
}

