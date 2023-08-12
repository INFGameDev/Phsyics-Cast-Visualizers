//@INF2023

using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
using EGL = UnityEditor.EditorGUILayout;
using GL = UnityEngine.GUILayout;
using EG = UnityEditor.EditorGUI;
#endif

public class BoxDividerAttribute : PropertyAttribute
{
    public string title;
    public bool isBold;
    public bool hasBorder;
    public Color backgroundColor;
    public Color borderColor;
    public int borderWidth;
    public Color textColor;

    private void SetDefaultColors()
    {
        if (EditorGUIUtility.isProSkin)
        {
            backgroundColor = new Color(0.8f, 0.8f, 0.8f, 1);
            borderColor = Color.black;
            textColor = new Color(0.1f, 0.1f, 0.1f, 1);
        }
        else
        {
            backgroundColor = new Color(0.3f, 0.3f, 0.3f, 1);
            borderColor = Color.white;
            textColor = new Color(0.9f, 0.9f, 0.9f, 1);
        }
    }

    public BoxDividerAttribute(string title)
    {
        this.title = title;
        isBold = true;
        hasBorder = true;
        borderWidth = 2;
        SetDefaultColors();
    }

    public BoxDividerAttribute(string title, int borderWidth = 2, bool isBold = true, bool hasBorder = true)
    {
        this.title = title;
        this.isBold = isBold;
        this.hasBorder = hasBorder;
        this.borderWidth = borderWidth;
        SetDefaultColors();
    }



    public BoxDividerAttribute(
        string title,
        Color backgroundColor,
        Color borderColor,
        Color textColor,
        int borderWidth = 2,
        bool isBold = true,
        bool hasBorder = true)
    {
        this.title = title;
        this.isBold = isBold;
        this.hasBorder = hasBorder;
        this.backgroundColor = backgroundColor;
        this.borderColor = borderColor;
        this.borderWidth = borderWidth;
        this.textColor = textColor;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(BoxDividerAttribute))]
public class BoxDividerAttributeDrawer : DecoratorDrawer
{
    private BoxDividerAttribute _attribute => (BoxDividerAttribute)attribute;
    private float layoutViewHeight = 45;
    private float verticalMargin = 20; // margin of the whole element container vertical

    public override float GetHeight() => layoutViewHeight;
    public override void OnGUI(Rect rect)
    {
        Rect borderBoxRect = rect;
        borderBoxRect.height -= verticalMargin; // apply margin
        borderBoxRect.y += verticalMargin / 2; // adjust center position

        DrawBorderBoxWithLabel(borderBoxRect, _attribute.title, _attribute.isBold, _attribute.textColor, _attribute.borderColor, _attribute.backgroundColor, _attribute.borderWidth);
    }

    private void DrawBorderBoxWithLabel(
        Rect rect,
        string title,
        bool isBold,
        Color textColor,
        Color borderColor,
        Color backgroundColor,
        int width = 2)
    {

        // render border
        Rect outer = new Rect(rect);
        EG.DrawRect(outer, borderColor); // draw rect

        // render inside fill
        Rect inner = new Rect(rect.x + width, rect.y + width, rect.width - width * 2, rect.height - width * 2);
        EG.DrawRect(inner, backgroundColor); // draw rect in front of rect border

        // render text
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.MiddleCenter;
        style.fontStyle = isBold ? FontStyle.Bold : FontStyle.Normal;
        style.normal.textColor = textColor;
        style.hover.textColor = textColor;
        style.active.textColor = textColor;
        GUI.Box(inner, title, style);
    }
}
#endif