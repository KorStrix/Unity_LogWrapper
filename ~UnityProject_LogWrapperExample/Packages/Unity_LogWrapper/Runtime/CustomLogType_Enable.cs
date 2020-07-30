using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Wrapper;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

[Serializable]
public class CustomLogType_Enable
{
    public string strCustomLogName;
    public bool bEnable = true;

    /// <summary>
    /// <see cref="DebugWrapperEditorSetting"/>에 있는 <see cref="CustomLogType"/>와 내용이 다를 시 arrMatchTarget를 변경시킵니다.
    /// </summary>
    /// <param name="pEditorSetting"></param>
    /// <param name="arrMatchTarget"></param>
    /// <returns>내용물이 서로 틀리면 true</returns>
    public static bool DoMatch_LogTypeEnableArray(DebugWrapperEditorSetting pEditorSetting, ref CustomLogType_Enable[] arrMatchTarget)
    {
        string[] arrLogTypeName_EditorSetting = pEditorSetting.arrLogType.Select(p => p.strLogTypeName).ToArray();
        string[] arrLogTypeName_PlayerPrefs = arrMatchTarget.Select(p => p.strCustomLogName).ToArray();

        var arrIntersect = arrLogTypeName_EditorSetting.Intersect(arrLogTypeName_PlayerPrefs);
        bool bIsRequireUpdate_LogTypeEnableArray = arrIntersect.Count() != arrLogTypeName_EditorSetting.Length;

        if (bIsRequireUpdate_LogTypeEnableArray)
        {
            List<CustomLogType_Enable> listLogTypeEnable = new List<CustomLogType_Enable>();
            foreach (CustomLogType pLogType in pEditorSetting.arrLogType)
            {
                var pLogTypeEnable = arrMatchTarget.FirstOrDefault(p => p.strCustomLogName == pLogType.strLogTypeName);
                if (pLogTypeEnable == null)
                    pLogTypeEnable = new CustomLogType_Enable(pLogType.strLogTypeName);

                listLogTypeEnable.Add(pLogTypeEnable);
            }

            arrMatchTarget = listLogTypeEnable.ToArray();
        }

        return bIsRequireUpdate_LogTypeEnableArray;
    }

    public CustomLogType_Enable(string strCustomLogName)
    {
        this.strCustomLogName = strCustomLogName;
    }


//    #region Editor
//    private const float fLabelWidth_strCustomLogName = 200f;
//    private const float fLabelWidth_bEnable = 50f;
//    private const float fLabelOffset = 5f;

//    public static void DoDrawEditorGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//#if UNITY_EDITOR
//        EditorGUI.BeginProperty(position, label, property);
//        {
//            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

//            SerializedProperty pProperty_strCustomLogName = property.FindPropertyRelative(nameof(CustomLogType_Enable.strCustomLogName));
//            EditorGUI.PropertyField(CalculateRect(ref position, fLabelWidth_strCustomLogName, fLabelOffset), pProperty_strCustomLogName, GUIContent.none);

//            SerializedProperty pProperty_bEnable = property.FindPropertyRelative(nameof(CustomLogType_Enable.bEnable));
//            EditorGUI.PropertyField(CalculateRect(ref position, fLabelWidth_bEnable, fLabelOffset), pProperty_bEnable, GUIContent.none);

//            label.text = $"{pProperty_strCustomLogName.stringValue}[{pProperty_bEnable.boolValue}]";
//        }
//        EditorGUI.EndProperty();
//#endif
//    }


//    private static Rect CalculateRect(ref Rect position, float fLabelWidth, float fOffset)
//    {
//        var rectFlagName = new Rect(position.x, position.y, fLabelWidth, position.height);

//        position.x += fLabelWidth + fOffset;

//        return rectFlagName;
//    }

//    #endregion
}


#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(CustomLogType_Enable))]
public class CustomLogType_EnableDrawer : PropertyDrawer
{
    private const float fLabelWidth_strCustomLogName = 200f;
    private const float fLabelWidth_bEnable = 50f;
    private const float fLabelOffset = 5f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        {
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            SerializedProperty pProperty_strCustomLogName = property.FindPropertyRelative(nameof(CustomLogType_Enable.strCustomLogName));
            EditorGUI.PropertyField(CalculateRect(ref position, fLabelWidth_strCustomLogName, fLabelOffset), pProperty_strCustomLogName, GUIContent.none);

            SerializedProperty pProperty_bEnable = property.FindPropertyRelative(nameof(CustomLogType_Enable.bEnable));
            EditorGUI.PropertyField(CalculateRect(ref position, fLabelWidth_bEnable, fLabelOffset), pProperty_bEnable, GUIContent.none);

            label.text = $"{pProperty_strCustomLogName.stringValue}[{pProperty_bEnable.boolValue}]";
        }
        EditorGUI.EndProperty();
    }

    private static Rect CalculateRect(ref Rect position, float fLabelWidth, float fOffset)
    {
        var rectFlagName = new Rect(position.x, position.y, fLabelWidth, position.height);

        position.x += fLabelWidth + fOffset;

        return rectFlagName;
    }
}
#endif
