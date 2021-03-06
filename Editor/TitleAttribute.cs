﻿/*--------------------------------------------------------------------
 * Author   Name: DXL
 * Creation Time: 2018.10.11
 * File Describe: Inspector面板上高级显示，例如显示中文
 * ------------------------------------------------------------------*/

using System;
using UnityEngine;

//namespace DapanzAI
//{
    //显示中文
    [AttributeUsage(AttributeTargets.Field)]
    public class Name : PropertyAttribute
    {
        public string label;        //要显示的字符
        public Name(string label)
        {
            this.label = label;
        }

    }


    //显示颜色
    public class TitleAttribute : PropertyAttribute
    {
        /// <summary> 标题名称 </summary>
        public string title;
        /// <summary> 标题颜色 </summary>
        public string htmlColor;

        /// <summary> 在属性上方添加一个标题 </summary>
        /// <param name="title">标题名称</param>
        /// <param name="htmlColor">标题颜色</param>
        public TitleAttribute(string title, string htmlColor = "#FFFFFF")
        {
            this.title = title;
            this.htmlColor = htmlColor;
        }

    }


    //显示枚举名称
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumName : PropertyAttribute
    {
        /// <summary> 枚举名称 </summary>
        public string name;
        public bool isFlag;
        public new int[] order = new int[0];

        public EnumName(string name)
        {
            this.name = name;
        }

        public EnumName(string label, params int[] order)
        {
            this.name = label;
            this.order = order;
        }

        public EnumName(string label, bool isFlag)
        {
            this.name = label;
            this.isFlag = isFlag;
        }
    }
//}