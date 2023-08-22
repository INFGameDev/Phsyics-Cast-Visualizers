// @INF 2023

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace INFAttributes
{
    /// <summary>
    /// makes a fold-out always expanded
    /// </summary>
    public class AlwaysExpandedAttribute : PropertyAttribute {}

    #if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(AlwaysExpandedAttribute))]
    public class AlwaysExpandedAttributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => EditorGUI.GetPropertyHeight(property, label, true);

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label, true);
            property.isExpanded = true;
        }
    }
    #endif
}

