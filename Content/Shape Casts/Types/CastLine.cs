using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysCastVisualier
{
    [AddComponentMenu("Physics Cast Visualizer/Shape Casts/Cast Line")]
    public class CastLine : ShapeCastVisualizer
    {
        public enum LinePointSource { Transform, Vector }
        
        [BoxDivider("Cast Line Properties")]
        [SerializeField] private LinePointSource linePointSource;
        [SerializeField] private Transform transformStart;
        [SerializeField] private Transform transformEnd;
        [SerializeField, DisplayOnly] private Vector3 vectorStart;
        [SerializeField, DisplayOnly] private Vector3 vectorEnd;

        [Header("Debug")]
        [SerializeField, DisplayOnly] private Vector3 lineStartPos;
        [SerializeField, DisplayOnly] private Vector3 lineEndPos;

        protected override bool Cast()
        {
            lineStartPos = linePointSource == LinePointSource.Transform ? transformStart.position : vectorStart;
            lineEndPos = linePointSource == LinePointSource.Transform ? transformEnd.position : vectorEnd;

            casting = true; 
            castTimeFrame = Time.frameCount;
            bool hasHitNow = false;

            if (Physics.Linecast(lineStartPos, lineEndPos, out hitResult, collidingLayers, GetTriggerInteraction()))
                hasHitNow = CheckTags();

            return hasHitNow;
        }

        public RaycastHit ManualCast(LayerMask targetLayers, bool detectTriggers)
        {
            this.collidingLayers = targetLayers;
            this.GetTriggerInteraction(detectTriggers);
            EventCheck(Cast());
            return hitResult;
        }

        public void MoveTransformStartLine(Vector3 pos) => transformStart.position = pos;
        public void MoveTransformEndLine(Vector3 pos) => transformEnd.position = pos;
        public void SetTransformStartLine(Vector3 pos) => transformStart.position = pos;
        public void SetTransformEndLine(Vector3 pos) => transformEnd.position = pos;
        public void SetVectorStartLine(Vector3 pos) => vectorStart = pos;
        public void SetVectorEndLine(Vector3 pos) => vectorEnd = pos;
        public void SetLinePointSource(LinePointSource source) => linePointSource = source;

        protected override void OnValidate() {
            base.OnValidate();
            hideCastDirectionField = true;
            hideMaxDistanceField = true;
            hideDirectionOriginOffsetField = true;
        }

        protected override void OnDrawGizmos()
        {
            if (!visualize)
                return;

            Vector3 s = Application.isPlaying ? lineStartPos : transformStart.position;
            Vector3 e = Application.isPlaying ? lineEndPos : transformEnd.position;

            Gizmos.color = GetDebugColor();
            Gizmos.DrawWireSphere(s, 0.05f);
            Gizmos.DrawWireSphere(e, 0.05f);
            Gizmos.DrawLine(s, e);
            Gizmos.color = default;
        }
    }

}

