/*Author   Name: DXL
* Creation Time: 2018.10.11
* File Describe: 实现InspectorShow里的方法，实现Inspector面板上的高级显示
* ------------------------------------------------------------------*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

//显示中文枚举
[CustomPropertyDrawer(typeof(EnumNameAttribute))]
public class EnumNameDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 替换属性名称
        EnumNameAttribute rename = (EnumNameAttribute)attribute;
        label.text = rename.name;

        // 重绘GUI
        bool isElement = Regex.IsMatch(property.displayName, "Element \\d+");
        if (isElement) label.text = property.displayName;
        if (property.propertyType == SerializedPropertyType.Enum)
        {
            drawEnum(position, property, label);
        }
        else
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    // 绘制枚举类型
    private void drawEnum(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginChangeCheck();

        // 获取枚举相关属性
        Type type = fieldInfo.FieldType;
        string[] names = property.enumNames;
        string[] values = new string[names.Length];
        while (type.IsArray) type = type.GetElementType();

        // 获取枚举所对应的名称
        for (int i = 0; i < names.Length; i++)
        {
            FieldInfo info = type.GetField(names[i]);
            EnumNameAttribute[] atts = (EnumNameAttribute[])info.GetCustomAttributes(typeof(EnumNameAttribute), false);
            values[i] = atts.Length == 0 ? names[i] : atts[0].name;
        }

        // 重绘GUI
        int index = EditorGUI.Popup(position, label.text, property.enumValueIndex, values);
        if (EditorGUI.EndChangeCheck() && index != -1) property.enumValueIndex = index;
    }

}