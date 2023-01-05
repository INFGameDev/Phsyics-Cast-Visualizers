using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PhysicsCastVisualizer
{
    public abstract class CastVisualizer<T> : MonoBehaviour 
    {
        [SerializeField] protected bool visualize;
        [SerializeField] protected LayerMask collidingLayers;
        [SerializeField] protected bool detectTriggers;
        [SerializeField] protected bool useParentRot;
        [SerializeField] protected CastDirection direction;

        [field: SerializeField] public string ID {get; protected set;}
        [field: SerializeField, DisplayOnly] public bool hasHit {get; protected set;}


        public bool autoCast = true;

        protected Vector3 rotationOffset;
        protected Vector3 relativePosition;
        protected Quaternion rotation;

        public enum CastDirection { Forward, Back, Right, Left, Up, Down }
        protected Vector3[] GlobalCastDirections = { Vector3.forward, Vector3.back, Vector3.right, Vector3.left, Vector3.up, Vector3.down };

        public void Visualize(bool b) => visualize = b;
        public override string ToString() => String.Format("{0}| {1}", gameObject.name, ID);
        protected QueryTriggerInteraction EvaluateTriggerDetection() => detectTriggers ? QueryTriggerInteraction.Collide : UnityEngine.QueryTriggerInteraction.Ignore;
        protected abstract void AutoCast();
        public virtual T ManualCast()
        {
            autoCast = false;
            return default(T);
        }
        protected virtual void Update() 
        {
            rotation = useParentRot ? transform.parent.rotation : transform.rotation;
            if (autoCast)
                AutoCast();
        }

        protected virtual void OnDrawGizmos() => rotation = useParentRot ? transform.parent.rotation : transform.rotation;
    }

    #region Attributes ====================================================================================================
    public class DisplayOnlyAttribute : PropertyAttribute
    {
        public string boolField;
        public DisplayOnlyAttribute(string boolVar = "") => this.boolField = boolVar;
    }

    #if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(DisplayOnlyAttribute))]
        public class DisplayOnlyAttributeDrawer : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                DisplayOnlyAttribute DOA = attribute as DisplayOnlyAttribute;
                bool render = false;

                if (DOA != null && DOA.boolField != string.Empty )
                {
                    var baseProperty = property.serializedObject.FindProperty(DOA.boolField);

                    if (baseProperty == null){
                        int basePath = property.propertyPath.LastIndexOf('.');
                        string fullPath = property.propertyPath.Substring(0, basePath+1) + DOA.boolField;

                        // Debug.Log(fullPath);
                        render = !property.serializedObject.FindProperty(fullPath).boolValue;
                        // Debug.Log(render);
                    } else {
                        render = !baseProperty.boolValue;
                    }
                }
                    

                GUI.enabled = render;
                EditorGUI.PropertyField(position, property, label, true);
                GUI.enabled = true;
            }
        }
    #endif

	public class TagsSelectionAttribute : PropertyAttribute{}

    #if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(TagsSelectionAttribute))]
    public class TagsSelectionAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                Debug.LogError($"Tags Selection Variable ({property.name}) in ({property.serializedObject.targetObject}) is expected to be a string");
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            property.stringValue = EditorGUI.TagField(position, label, property.stringValue);
        }
    }
    #endif

    #endregion  Attributes ====================================================================================================


}


