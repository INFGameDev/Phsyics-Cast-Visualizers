// @INF 2023

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
using INFAttributesExtensionMethods;
#endif

public class DisplayOnlyAttribute : DrawConditionAttribute
{
    /// <summary>
    /// [ Allowed field types ]
    /// Unity Object fields -> checks if null or not
    /// Boolean -> if it's true or not
    /// </summary>
    /// <param name="fieldEnabler">a SERIALIZED field (UnityObject or Boolean) that is checked to determine if the field state is gonna be readonly or not </param>
    public DisplayOnlyAttribute(string fieldEnabler) : base(fieldEnabler) {}

    /// <summary>
    /// [ Allowed field types ]
    /// Unity Object fields -> checks if null or not
    /// Boolean -> if it's true or not
    /// </summary>
    /// <param name="fieldEnabler">a SERIALIZED field (UnityObject or Boolean) that is checked to determine if the field state is gonna be readonly or not </param>
    /// <param name="inverseCheck">flips the condition</param>
    public DisplayOnlyAttribute(string fieldEnabler, bool inverseCheck) : base(fieldEnabler, inverseCheck) {}

    /// <summary>
    /// [ Allowed field types ]
    /// Enum -> checks if this enum field variable matches the enum constants being compared to it
    /// </summary>
    /// <param name="fieldEnabler">a SERIALIZED field (UnityObject or Boolean) that is checked to determine if the field state is gonna be readonly or not </param>
    /// <param name="inverseCheck">flips the condition</param>
    /// <param name="enumConstantsToCompare">one or multiple enum constants to compare with the enum field variable</param>
    public DisplayOnlyAttribute(string fieldEnabler, bool inverseCheck = false, params object[] enumConstantsToCompare) : base(fieldEnabler, inverseCheck, enumConstantsToCompare) {}
    
    public DisplayOnlyAttribute() { }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(DisplayOnlyAttribute))]
public class DisplayOnlyAttributeDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        DisplayOnlyAttribute doa = attribute as DisplayOnlyAttribute;
        GUI.enabled = doa.GetDrawConditionResult(property);
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}
#endif