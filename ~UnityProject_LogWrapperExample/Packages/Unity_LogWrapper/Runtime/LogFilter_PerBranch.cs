using System;
using System.Reflection;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class LogFilter_PerBranch
{
    public const string const_strPlayerPefs_SaveKey = nameof(LogFilter_PerBranch);

    private static string const_strFormat =
@"#if {0}

{1}
#endif";


    public LogWrapperEditorSetting pEditorSetting;

    public string strBranchName;
    public CustomLogType_Enable[] arrLogTypeEnable;

    public static LogFilter_PerBranch Get_LogTypeEnable_FromPlayerPrefs(out bool bIsFail)
    {
        LogFilter_PerBranch pLocalBranch = new LogFilter_PerBranch();
        bIsFail = LogWrapperUtility.Load_FromPlayerPrefs(LogFilter_PerBranch.const_strPlayerPefs_SaveKey, ref pLocalBranch) == false;

        return pLocalBranch;
    }

    public CustomLogType[] GetEnableLogType()
    {
        if (pEditorSetting == null)
        {
            Debug.LogError($"GetEnableLogType - pEditorSetting == null");
            return new CustomLogType[0];
        }

        CustomLogType[] arrLogType = pEditorSetting.arrLogType;
        
        return arrLogTypeEnable
            .Where(p => p.bEnable)
            .Select(p => arrLogType.FirstOrDefault(pLogType => pLogType.LogTypeName.Equals(p.strCustomLogName)))
            .Where(p => p != null)
            .ToArray();
    }

    public string ToCSharpCodeString(string strListFieldName)
    {
        return string.Format(const_strFormat, strBranchName, ToCSharpCode_AddList(strListFieldName));
    }

    private string ToCSharpCode_AddList(string strListFieldName)
    {
        StringBuilder strBuilder = new StringBuilder();

        var arrEnableLogType = arrLogTypeEnable.Where(p => p.bEnable);
        foreach(var pLogType in arrEnableLogType)
            strBuilder.AppendLine($"            {strListFieldName}.Add({pLogType.strCustomLogName});");

        return strBuilder.ToString();
    }
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

            LogWrapperEditorSetting pEditorSetting = pProperty_pEditorSetting.objectReferenceValue as LogWrapperEditorSetting;
            if (pEditorSetting == null)
                return;

            position.y += const_fHeightPerLine;
            EditorGUI.indentLevel++;
            {
                SerializedObject pSO = property.serializedObject;
                LogFilter_PerBranch pBranch = GetThis(property);

                if (CustomLogType_Enable.DoMatch_LogTypeEnableArray(pEditorSetting, ref pBranch.arrLogTypeEnable))
                {
                    pSO.ApplyModifiedProperties();
                    EditorUtility.SetDirty(pSO.targetObject);
                }

                SerializedProperty pProperty_arrLogTypeEnable = property.FindPropertyRelative(nameof(LogFilter_PerBranch.arrLogTypeEnable));
                EditorGUI.PropertyField(position, pProperty_arrLogTypeEnable, true);
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
        object pValue = pFieldInfo_Array.GetValue(pObjectOwner);
        if (pValue is Array pArray)
        {
            string strValue = Regex.Match(property.propertyPath, @"\d+").Value;
            int iIndex = int.Parse(strValue);

            return (LogFilter_PerBranch)pArray.GetValue(iIndex);
        }

        return (LogFilter_PerBranch)pValue;
    }
}
#endif
