
//#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
/// <summary>
/// 能让字段在inspect面板显示中文字符
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class FieldLabelAttribute : PropertyAttribute
{
    public string label;//要显示的字符
    public FieldLabelAttribute(string label)
    {
        this.label = label;
        //获取你想要绘制的字段（比如"技能"）
    }
}
//绑定特性描述类
[CustomPropertyDrawer(typeof(FieldLabelAttribute))]
public class FieldLabelDrawer : PropertyDrawer
{
    private FieldLabelAttribute FLAttribute
    {
        get { return (FieldLabelAttribute)attribute; }
        ////获取你想要绘制的字段
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //在这里重新绘制
        EditorGUI.PropertyField(position, property, new GUIContent(FLAttribute.label), true);
    }
}
//#endif