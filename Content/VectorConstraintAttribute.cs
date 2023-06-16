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
		
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) 
		{
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