// @INF

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PhysCastVisualier
{
    public abstract class CastVisualizer<T> : MonoBehaviour 
    {
        public enum CastDirection { Forward, Back, Right, Left, Up, Down }
        protected Vector3[] GlobalCastDirections = { Vector3.forward, Vector3.back, Vector3.right, Vector3.left, Vector3.up, Vector3.down };

        [BoxDivider("Visualizer Properties")]

        [SerializeField] protected bool visualize;

        [SerializeField] protected LayerMask collidingLayers;
        [SerializeField] protected bool detectTriggers;
        [SerializeField] protected CastDirection direction;
        [SerializeField] protected bool autoCast = true;
        [SerializeField] protected Color castColor = Color.white;
        [SerializeField] protected Color hasHitColor = Color.red;
 
        [field: SerializeField, DisplayOnly] public bool hasHit {get; protected set;} = false;

        protected Vector3 rotationOffset;
        protected Vector3 relativePosition;

        public void Visualize(bool b) => visualize = b;
        protected QueryTriggerInteraction GetTriggerInteraction() => detectTriggers ? QueryTriggerInteraction.Collide : UnityEngine.QueryTriggerInteraction.Ignore;
        
        protected abstract void AutoCast();

        public virtual T ManualCast()
        {
            autoCast = false;
            return default(T);
        }
        
        protected virtual void Update() 
        {
            if (autoCast)
                AutoCast();
        }

        protected virtual void OnDrawGizmos(){}
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


