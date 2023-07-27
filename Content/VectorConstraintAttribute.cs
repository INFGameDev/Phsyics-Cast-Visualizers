using UnityEngine;
using UnityEditor;
using System;

#if UNITY_EDITOR
using EGL = UnityEditor.EditorGUILayout;
#endif

namespace PhysCastVisualier
{
	public static class VectorConstraintAttributeExtensions
	{
		public static float Clamp(this float f, float min, float max) => Mathf.Clamp(f, min, max);
	}

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
			// string[] enumNames = direction.enumNames;
			float[] extents = new float[2];
			byte[] fill = new byte[3] {0, 0, 0};
			GUIContent[] subLabels = new GUIContent[] { new GUIContent(String.Empty), new GUIContent(String.Empty) };
			

			switch (direction.enumValueIndex)
			{
				case 0: // Forward
				case 1: // Back
					extents = new float[] {property.vector3Value.x, property.vector3Value.y};
					fill = new byte[] {1, 1, 0};
					subLabels[0].text = "X";
					subLabels[1].text = "Y";
					break;
				
				case 2: // Right
				case 3: // Left
					extents = new float[] {property.vector3Value.y, property.vector3Value.z};
					fill = new byte[] {0, 1, 1};
					subLabels[0].text = "Y";
					subLabels[1].text = "Z";
					break;
				case 4: // Up
				case 5: // DOwn
					extents = new float[] {property.vector3Value.x, property.vector3Value.z};
					fill = new byte[] {1, 0, 1};
					subLabels[0].text = "X";
					subLabels[1].text = "Z";
					break;
			}

			EditorGUI.MultiFloatField(
				position, 
				new GUIContent("Extents"), 
				subLabels, 
				extents
			);

			property.vector3Value = new Vector3(
				fill[0] == 0 ? 0 : extents[0].Clamp(0, Mathf.Infinity), 
				fill[1] == 0 ? 0 : fill[2] == 0 ? extents[1].Clamp(0, Mathf.Infinity) : extents[0].Clamp(0, Mathf.Infinity), 
				fill[2] == 0 ? 0 : extents[1].Clamp(0, Mathf.Infinity));

			// property.vector3Value = new Vector3(extents[0], extents[1]);

			// if (direction.enumValueIndex == 0 || direction.enumValueIndex == 1) // FORWARD | BACK
			// {
			// 	Vector3 v = Constrain(
			// 		EGL.FloatField("Extent X", property.vector3Value.x),
			// 		EGL.FloatField("Extent Y", property.vector3Value.y),
			// 		null
			// 	);
			// 	property.vector3Value = v;
			// } 
			// else if (direction.enumValueIndex == 2 || direction.enumValueIndex == 3) // RIGHT | LEFT
			// {
			// 	Vector3 v = Constrain(
			// 		null,
			// 		EGL.FloatField("Extent Y", property.vector3Value.y),
			// 		EGL.FloatField("Extent Z", property.vector3Value.z)
			// 	);
			// 	property.vector3Value = v;
			// } 
			// else if (direction.enumValueIndex == 4 || direction.enumValueIndex == 5) // UP | DOWN
			// {
			// 	Vector3 v = Constrain(
			// 		EGL.FloatField("Extent X", property.vector3Value.x),
			// 		null,
			// 		EGL.FloatField("Extent Z", property.vector3Value.z)
			// 	);
			// 	property.vector3Value = v;
			// }

			property.serializedObject.ApplyModifiedProperties();
		}

		// private Vector3 Constrain(float? x, float? y, float? z)
		// {
		// 	Vector3 newVector = new Vector3(x == null ? 0 : x.Value, y == null ? 0 : y.Value, z == null ? 0 : z.Value);
		// 	return newVector;
		// }
	}
	#endif	
}