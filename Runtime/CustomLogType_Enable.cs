using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class CustomLogType_Enable
{
    public string strCustomLogName;
    public bool bEnable;

    /// <summary>
    /// <see cref="LogWrapperSetting"/>에 있는 <see cref="CustomLogType"/>와 내용이 다를 시 arrMatchTarget를 변경시킵니다.
    /// </summary>
    /// <param name="pSetting"></param>
    /// <param name="arrMatchTarget"></param>
    /// <returns>내용물이 서로 틀리면 true</returns>
    public static bool DoMatch_LogTypeEnableArray(LogWrapperSetting pSetting, ref CustomLogType_Enable[] arrMatchTarget)
    {
        if (arrMatchTarget == null)
            arrMatchTarget = new CustomLogType_Enable[0];

        string[] arrLogTypeName_EditorSetting = pSetting.arrLogType.Select(p => p.strLogTypeName).ToArray();
        string[] arrLogTypeName_Target = arrMatchTarget.Select(p => p.strCustomLogName).ToArray();

        var arrIntersect = arrLogTypeName_EditorSetting.Intersect(arrLogTypeName_Target);
        int iCount = arrIntersect.Count();

        bool bIsRequireUpdate_LogTypeEnableArray = iCount != arrLogTypeName_EditorSetting.Length || iCount != arrLogTypeName_Target.Length;

        if (bIsRequireUpdate_LogTypeEnableArray)
        {
            List<CustomLogType_Enable> listLogTypeEnable = new List<CustomLogType_Enable>();
            foreach (CustomLogType pLogType in pSetting.arrLogType)
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
}


#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(CustomLogType_Enable))]
public class CustomLogType_EnableDrawer : PropertyDrawer
{
    private const float fLabelWidth_strCustomLogName = 200f;
    private const float fLabelOffset = 5f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        {
            // position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            // position.height = 20f;

            SerializedProperty pProperty_strCustomLogName = property.FindPropertyRelative(nameof(CustomLogType_Enable.strCustomLogName));
            // EditorGUI.LabelField(CalculateRect(ref position, fLabelWidth_strCustomLogName, fLabelOffset), pProperty_strCustomLogName.stringValue);
            EditorGUI.LabelField(CalculateRect(ref position, fLabelWidth_strCustomLogName, fLabelOffset), pProperty_strCustomLogName.stringValue);

            SerializedProperty pProperty_bEnable = property.FindPropertyRelative(nameof(CustomLogType_Enable.bEnable));
            EditorGUI.PropertyField(position, pProperty_bEnable, GUIContent.none);

            label.text = $"{pProperty_strCustomLogName.stringValue}[{pProperty_bEnable.boolValue}]";
        }
        EditorGUI.EndProperty();
    }

    private static Rect CalculateRect(ref Rect position, float fLabelWidth, float fOffset)
    {
        var rectFlagName = new Rect(position.x, position.y, fLabelWidth, position.height);

        position.x += (fLabelWidth * 0.5f) + fOffset;

        return rectFlagName;
    }
}
#endif
