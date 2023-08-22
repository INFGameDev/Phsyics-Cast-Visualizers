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
    public class DrawConditionAttribute : PropertyAttribute
    {
        public string fieldEnabler;
        public bool inverseCheck = false;
        public Enum[] enumConstantsCompare;

        /// <summary>
        /// [ Allowed field types ]
        /// Unity Object fields -> checks if null or not
        /// Boolean -> if it's true or not
        /// </summary>
        /// <param name="fieldEnabler">a SERIALIZED field (UnityObject or Boolean) that is checked to determine if the field state is gonna be readonly or not </param>
        public DrawConditionAttribute(string fieldEnabler) => this.fieldEnabler = fieldEnabler;

        /// <summary>
        /// [ Allowed field types ]
        /// Unity Object fields -> checks if null or not
        /// Boolean -> if it's true or not
        /// </summary>
        /// <param name="fieldEnabler">a SERIALIZED field (UnityObject or Boolean) that is checked to determine if the field state is gonna be readonly or not </param>
        /// <param name="inverseCheck">flips the condition</param>
        public DrawConditionAttribute(string fieldEnabler, bool inverseCheck)
        {
            this.inverseCheck = inverseCheck;
            this.fieldEnabler = fieldEnabler;
        }

        /// <summary>
        /// [ Allowed field types ]
        /// Enum -> checks if this enum field variable matches the enum constants being compared to it
        /// </summary>
        /// <param name="fieldEnabler">a SERIALIZED field (UnityObject or Boolean) that is checked to determine if the field state is gonna be readonly or not </param>
        /// <param name="inverseCheck">flips the condition</param>
        /// <param name="enumConstantsToCompare">one or multiple enum constants to compare with the enum field variable</param>
        public DrawConditionAttribute(string isEnabledBoolCheckField, bool inverseCheck = false, params object[] enumConstantsToCompare)
        {
            this.fieldEnabler = isEnabledBoolCheckField;
            this.enumConstantsCompare = new Enum[enumConstantsToCompare.Length];
            this.inverseCheck = inverseCheck;

            for (int i = 0; i < enumConstantsToCompare.Length; i++) {
                enumConstantsCompare[i] = (Enum)enumConstantsToCompare[i];
            }
        }
        
        public DrawConditionAttribute() { }
    }

    #if UNITY_EDITOR
    public static class DrawConditionAttributeHelper
    {
        public static bool GetDrawConditionResult(this DrawConditionAttribute attribute, SerializedProperty fieldProperty)
        {
            bool enableField = false;
            if (attribute != null)
            {
                // check if there is inputed field
                if (!string.IsNullOrEmpty(attribute.fieldEnabler))
                {
                    var sp = fieldProperty.serializedObject.FindProperty(attribute.fieldEnabler);

                    // check if there such a field exist
                    if (sp == null)
                    {
                        Debug.LogError($"field ({attribute.fieldEnabler}) doesn't exist");
                        return false;
                    }

                    // determine what type is the property and perform the readonly outcome based on that property's state
                    switch (sp.propertyType)
                    {
                        case SerializedPropertyType.ObjectReference:
                            enableField = sp.objectReferenceValue != null;
                            break;
                        case SerializedPropertyType.Boolean:
                            enableField = sp.boolValue;
                            break;
                        case SerializedPropertyType.Enum:
                            for (int i = 0; i < attribute.enumConstantsCompare.Length; i++)
                            {
                                if (sp.enumValueIndex == Convert.ToInt32(attribute.enumConstantsCompare[i]))
                                {
                                    enableField = true;
                                    break;
                                }
                            }
                            break;
                        default:
                            Debug.LogError($"field type ({sp.propertyType}) is not supported");
                            break;
                    }

                    enableField = attribute.inverseCheck ? !enableField : enableField;
                }
            }

            return enableField;
        }
    }
    #endif
}
