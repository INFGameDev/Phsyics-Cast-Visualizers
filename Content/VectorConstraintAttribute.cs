using UnityEngine;
using UnityEditor;
using System;

#if UNITY_EDITOR
using EGL = UnityEditor.EditorGUILayout;
#endif

namespace PhysicsCastVisualizer
{
	public class VectorConstraintAttribute : PropertyAttribute
	{
		public string directionField;
		public VectorConstraintAttribute(string directionField) => this.directionField = directionField;
	}

	#if UNITY_EDITOR

	[CustomPropertyDrawer(typeof(VectorConstraintAttribute))]
	class VectorConstraintDrawer: PropertyDrawer {


			// public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
			// {
			// 	if (!(attribute is VectorConstraintAttribute conditional)) return 0;

			// 	Debug.Log(conditional.FieldToCheck == null);

			// 	return EditorGUI.GetPropertyHeight(property);
			// }

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) 
		{

			// ----------------------------------------------------------------------------------------------------------------------------
			// OnGUI runs at least twice per frame
			// One of those is to calculate layouts and the other is to actually draw stuff ("repaint")

			// EventType.Layout - This event is sent prior to anything else - this is a chance to perform any initialization. It is used by the automatic layout system.
			// EventType.Repaint - A repaint event. One is sent every frame, All other events are processed first, then the repaint event is sent.
			

			// it seems that on lower versionf of unity below 2021 or 2020; any kind of UI state modification is not allowed
			// while doing repaint but works fine on 2021 where this script is written on

			// so draw call is executed during the layout event
			if (Event.current.type != EventType.Layout)
				return;

			// ----------------------------------------------------------------------------------------------------------------------------

			VectorConstraintAttribute VCA = attribute as VectorConstraintAttribute;
			var direction = property.serializedObject.FindProperty(VCA.directionField);
			string[] enumNames = direction.enumNames;


			if (direction.enumValueIndex == 0 || direction.enumValueIndex == 1) // FORWARD | BACK
			{
				Vector3 v = Constrain(
					EGL.FloatField("Extent X", property.vector3Value.x),
					EGL.FloatField("Extent Y", property.vector3Value.y),
					null
				);

				property.vector3Value = v;

			} 
			else if (direction.enumValueIndex == 2 || direction.enumValueIndex == 3) // RIGHT | LEFT
			{
				Vector3 v = Constrain(
					null,
					EGL.FloatField("Extent Y", property.vector3Value.y),
					EGL.FloatField("Extent Z", property.vector3Value.z)
				);

				property.vector3Value = v;
			} 
			else if (direction.enumValueIndex == 4 || direction.enumValueIndex == 5) // UP | DOWN
			{
				Vector3 v = Constrain(
					EGL.FloatField("Extent X", property.vector3Value.x),
					null,
					EGL.FloatField("Extent Z", property.vector3Value.z)
				);

				property.vector3Value = v;
			}
		
			property.serializedObject.ApplyModifiedProperties();
		}

		private Vector3 Constrain(float? x, float? y, float? z)
		{
			Vector3 newVector = Vector3.zero;

			if (x == null) 
			{
				newVector.y = Mathf.Clamp(y.Value, 0, Mathf.Infinity);
				newVector.z = Mathf.Clamp(z.Value, 0, Mathf.Infinity);
			} 
			else if (y == null)
			{
				newVector.x = Mathf.Clamp(x.Value, 0, Mathf.Infinity);
				newVector.z = Mathf.Clamp(z.Value, 0, Mathf.Infinity);
			}
			else if (z == null)
			{
				newVector.x = Mathf.Clamp(x.Value, 0, Mathf.Infinity);
				newVector.y = Mathf.Clamp(y.Value, 0, Mathf.Infinity);
			}

			return newVector;
		}
	}
	#endif	
}

