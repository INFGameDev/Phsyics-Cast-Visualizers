// @INF 2023

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// render only the visible children (level 1 depth) of the class property
/// </summary>
public class ShowChildrenOnlyAttribute : PropertyAttribute {}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ShowChildrenOnlyAttribute))]
public class ShowChildrenOnlyAttributeDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => 0;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        IEnumerator enumerator = property.GetEnumerator(); // get all the visible child properties of the property
        while (enumerator.MoveNext()) // loop through all of them (this includes further nested children)
        {
            SerializedProperty sp = enumerator.Current as SerializedProperty; 

            if (sp.depth == 1) // only render level 1 deep property fields
                EditorGUILayout.PropertyField(sp);
        }
    }
}
#endif