// @INF 2023

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
using INFAttributesExtensionMethods;
#endif

public class DisplayIfAttribute : DrawConditionAttribute
{
    /// <summary>
    /// [ Allowed field types ]
    /// Unity Object fields -> checks if null or not
    /// Boolean -> if it's true or not
    /// </summary>
    /// <param name="fieldEnabler">a SERIALIZED field (UnityObject or Boolean) that is checked to determine if the field state is gonna be readonly or not </param>
    public DisplayIfAttribute(string fieldEnabler) : base(fieldEnabler) { }

    /// <summary>
    /// [ Allowed field types ]
    /// Unity Object fields -> checks if null or not
    /// Boolean -> if it's true or not
    /// </summary>
    /// <param name="fieldEnabler">a SERIALIZED field (UnityObject or Boolean) that is checked to determine if the field state is gonna be readonly or not </param>
    /// <param name="inverseCheck">flips the condition</param>
    public DisplayIfAttribute(string fieldEnabler, bool inverseCheck) : base(fieldEnabler, inverseCheck) { }

    /// <summary>
    /// [ Allowed field types ]
    /// Enum -> checks if this enum field variable matches the enum constants being compared to it
    /// </summary>
    /// <param name="fieldEnabler">a SERIALIZED field (UnityObject or Boolean) that is checked to determine if the field state is gonna be readonly or not </param>
    /// <param name="inverseCheck">flips the condition</param>
    /// <param name="enumConstantsToCompare">one or multiple enum constants to compare with the enum field variable</param>
    public DisplayIfAttribute(string fieldEnabler, bool inverseCheck = false, params object[] enumConstantsToCompare) : base(fieldEnabler, inverseCheck, enumConstantsToCompare) { }

    public DisplayIfAttribute() { }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(DisplayIfAttribute))]
public class DisplayIfAttributeDrawer : PropertyDrawer
{
    private bool drawField;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return drawField ? EditorGUI.GetPropertyHeight(property, label, true) : 0;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        DisplayIfAttribute dia = attribute as DisplayIfAttribute;
        drawField= dia.GetDrawConditionResult(property);

        if (drawField) 
            EditorGUI.PropertyField(position, property, label, true);
    }
}
#endif