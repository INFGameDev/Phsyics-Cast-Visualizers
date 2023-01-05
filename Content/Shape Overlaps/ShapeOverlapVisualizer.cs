using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsCastVisualizer
{
    public abstract class ShapeOverlapVisualizer : CastVisualizer<bool> 
    {
        [SerializeField] protected int detectionCountLimit = 1;
        [SerializeField, DisplayOnly] protected List<Collider> detectedColliders;
        [SerializeField, TagsSelection] protected string[] compareTags;
        [SerializeField] protected Mesh castMesh;
        [SerializeField] protected Vector3 offset;
        protected Collider[] initiallyDetectedColliders;
        protected Collider[] finalOutput;
        protected override void AutoCast() => Cast();

        public override bool ManualCast()
        {
            Cast();
            return hasHit;
        }

        protected abstract void Cast(); 
    }
}

