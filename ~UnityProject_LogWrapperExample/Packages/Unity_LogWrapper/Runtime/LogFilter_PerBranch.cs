using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Wrapper;
using Debug = UnityEngine.Debug;
using System.Linq;
using System.Text.RegularExpressions;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class LogFilter_PerBranch
{
    public DebugWrapperEditorSetting pEditorSetting;
    public string strBranchName;
    public CustomLogType_Enable[] arrLogTypeEnable;
}


#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(LogFilter_PerBranch))]
public class LogFilter_PerBranchDrawer : PropertyDrawer
{
    private const float const_fHeightPerLine = 18f;

    // EditorGUILayout을 쓰면 제대로 나오는데 인스펙터에서 잘 안나오고
    // EditorGUI를 쓰면 Array 계산을 해야함;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        {
            SerializedProperty pProperty_strBranchName = property.FindPropertyRelative(nameof(LogFilter_PerBranch.strBranchName));
            EditorGUI.PropertyField(position, pProperty_strBranchName, true);
            label.text = $"{pProperty_strBranchName.stringValue}";


            SerializedProperty pProperty_pEditorSetting = property.FindPropertyRelative(nameof(LogFilter_PerBranch.pEditorSetting));
            if (pProperty_pEditorSetting == null)
            {
                Debug.LogError($"{label.text} - Error pProperty_pEditorSetting == null");
                return;
            }

            DebugWrapperEditorSetting pEditorSetting = pProperty_pEditorSetting.objectReferenceValue as DebugWrapperEditorSetting;
            if (pEditorSetting == null)
                return;

            CustomLogType[] arrLogType = pEditorSetting.arrLogType;

            position.y += const_fHeightPerLine;
            EditorGUI.indentLevel++;
            {
                //LogFilter_PerBranch pBranch = GetThis(property);
                //CustomLogType_Enable[] arrLogTypeEnable = pBranch.arrLogTypeEnable;
                //CustomLogType_Enable.DoMatch_LogTypeEnableArray(pEditorSetting, ref arrLogTypeEnable);

                //for (int i = 0; i < arrLogTypeEnable.Length; i++)
                //{
                //    SerializedProperty pPropertyElement = pProperty_arrLogTypeEnable.GetArrayElementAtIndex(i);
                //    // arrLogTypeEnable[i].DoDrawEditorGUI(position, pPropertyElement);
                //}

                SerializedProperty pProperty_arrLogTypeEnable = property.FindPropertyRelative(nameof(LogFilter_PerBranch.arrLogTypeEnable));
                EditorGUI.PropertyField(position, pProperty_arrLogTypeEnable, true);

                //for (int i = 0; i < pProperty_arrLogTypeEnable.arraySize; i++)
                //{
                //    SerializedProperty pPropertyElement = pProperty_arrLogTypeEnable.GetArrayElementAtIndex(i);
                //    EditorGUI.PropertyField(position, pPropertyElement, true);
                //    position.y += const_fHeightPerLine;
                //}
            }
            EditorGUI.indentLevel--;

        }
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty pProperty_arrLogTypeEnable = property.FindPropertyRelative(nameof(LogFilter_PerBranch.arrLogTypeEnable));
        int iArrayCount = pProperty_arrLogTypeEnable.arraySize;

        float fDefaultSize = base.GetPropertyHeight(property, label) + const_fHeightPerLine;
        if (pProperty_arrLogTypeEnable.isExpanded)
            fDefaultSize += (const_fHeightPerLine * (iArrayCount + 1));
            
        return fDefaultSize;
    }


    private LogFilter_PerBranch GetThis(SerializedProperty property)
    {
        object pObjectOwner = property.serializedObject.targetObject;
        Type pOwnerType = pObjectOwner.GetType();
        FieldInfo pFieldInfo_Array = pOwnerType.GetField(property.propertyPath.Split('.').FirstOrDefault());
        Array pTest = (Array)pFieldInfo_Array.GetValue(pObjectOwner);
        string strValue = Regex.Match(property.propertyPath, @"\d+").Value;
        int iIndex = int.Parse(strValue);

        return (LogFilter_PerBranch)pTest.GetValue(iIndex);
    }
}
#endif
