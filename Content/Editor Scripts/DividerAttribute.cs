using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
    using EGL = UnityEditor.EditorGUILayout;
    using GL = UnityEngine.GUILayout;
    using EG = UnityEditor.EditorGUI;
#endif

public class DividerAttribute : PropertyAttribute
{
    public readonly string label;
    public readonly bool WithOffset;

    public DividerAttribute()
    {
        label = string.Empty;
    }

    public DividerAttribute(string title, bool withOffset = false)
    {
        label = title;
        WithOffset = withOffset;
    }
}


#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(DividerAttribute))]
    public class DividerAttributeDrawer : DecoratorDrawer
    {
        private DividerAttribute Separator => (DividerAttribute)attribute;

        public override float GetHeight() => Separator.WithOffset ? 40 : string.IsNullOrEmpty(Separator.label) ? 20 : 25;
        
        public override void OnGUI(Rect rect)
        {
            if (rect.width != 1)
            {
                var title = Separator.label;
                if (string.IsNullOrEmpty(title))
                {
                    rect.height = 1;
                    rect.y += 14;
                    GUI.Box(rect, string.Empty);
                }
                else
                {
                    Vector2 wrappedTextSize = GUI.skin.label.CalcSize(new GUIContent(title));
                    float horizontalMargin = 10;
                    float leftDividerLineWidth = (rect.width - wrappedTextSize.x) / 2 - horizontalMargin;
                    float lineHeight = 1;

                    EditorGUI.DrawRect( new Rect(rect.xMin, rect.y + rect.height / 2, leftDividerLineWidth, lineHeight), Color.blue );
                    GUI.Label(new Rect(rect.xMin + leftDividerLineWidth + horizontalMargin, ( rect.y + rect.height / 2) - (wrappedTextSize.y/2), wrappedTextSize.x, wrappedTextSize.y), title);
                    EditorGUI.DrawRect( new Rect(rect.xMin + leftDividerLineWidth + wrappedTextSize.x + (horizontalMargin * 2),  rect.y + rect.height / 2, leftDividerLineWidth, lineHeight), Color.red );
                }
            }
        }
    }
#endif