// @INF 2023

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
			bool drawField = true;
			float[] extents = new float[2];
			byte[] fill = new byte[3] {0, 0, 0};
			GUIContent[] subLabels = new GUIContent[] { new GUIContent(String.Empty), new GUIContent(String.Empty) };
			

			switch (direction.enumValueIndex)
			{
				case 1: // Forward
				case 2: // Back
					extents = new float[] {property.vector3Value.x, property.vector3Value.y};
					fill = new byte[] {1, 1, 0};
					subLabels[0].text = "X";
					subLabels[1].text = "Y";
					break;
				
				case 3: // Right
				case 4: // Left
					extents = new float[] {property.vector3Value.y, property.vector3Value.z};
					fill = new byte[] {0, 1, 1};
					subLabels[0].text = "Y";
					subLabels[1].text = "Z";
					break;
				case 5: // Up
				case 6: // DOwn
					extents = new float[] {property.vector3Value.x, property.vector3Value.z};
					fill = new byte[] {1, 0, 1};
					subLabels[0].text = "X";
					subLabels[1].text = "Z";
					break;
				default:
					drawField = false;
					break;
			}

			if (drawField == false)
			{
				EditorGUILayout.HelpBox("There must be a direction!, \n it can't be none it's a physics cast for gawd sake you idiot! (╯°□°）╯︵ ┻━┻", MessageType.Error);
				return;
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

			property.serializedObject.ApplyModifiedProperties();
		}
	}
	#endif	
}