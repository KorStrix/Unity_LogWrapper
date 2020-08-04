using System;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// LogWrapper에 대한 세팅값들
/// <para><see cref="Resources.Load(string)"/>를 통해 얻어오기 때문에 Resources 폴더 안에 있어야 합니다.</para>
/// </summary>
[Serializable]
public class LogWrapperEditorSetting : ScriptableObject
{
    public static LogWrapperEditorSetting pCurrentSetting
    {
        get
        {
            if (_pCurrentSetting == null)
            {
                LogWrapperEditorSetting[] arrSetting = Resources.LoadAll<LogWrapperEditorSetting>("");

                int iCurrentSettingCount = arrSetting.Count(p => p.bIsCurrent);
                if (iCurrentSettingCount > 1)
                {
                    UnityEngine.Debug.LogWarning($"{nameof(LogWrapperEditorSetting)} - iCurrentSettingCount({iCurrentSettingCount}) > 1");
                }

                _pCurrentSetting = arrSetting.FirstOrDefault(p => p.bIsCurrent);
                if (_pCurrentSetting == null)
                {
                    // 일단 현재 존재하는 것 중에 찾아서 넣습니다.
                    _pCurrentSetting = arrSetting.FirstOrDefault();
                    if (_pCurrentSetting == null)
                    {
                        if (Application.isEditor)
                        {
                            _pCurrentSetting = LogWrapperUtility.CreateAsset<LogWrapperEditorSetting>();
                            UnityEngine.Debug.Log($"{nameof(LogWrapperEditorSetting)} is null / auto create default setting", _pCurrentSetting);
                        }
                        else
                        {
                            _pCurrentSetting = CreateInstance<LogWrapperEditorSetting>();
                            UnityEngine.Debug.LogWarning($"{nameof(LogWrapperEditorSetting)} is null / auto create default setting");
                        }
                    }

                    _pCurrentSetting.bIsCurrent = true;
                }
            }

            return _pCurrentSetting;
        }
    }

    static LogWrapperEditorSetting _pCurrentSetting;

    public bool bIsCurrent;
    public string strTypeName;
    public string strCSExportPath;
    public CustomLogType[] arrLogType = new CustomLogType[0];

    // 여기에 브렌치별 Log 필터 정보도 넣어야 할듯?
    public LogFilter_PerBranch[] arrBranch;


#if UNITY_EDITOR
    public static void DoDrawEditorGUI(SerializedObject pSerializeObject, LogWrapperEditorSetting pEditorSetting)
    {
        pSerializeObject.Update();

        bool bIsSet_EditorSetting = false;

        EditorGUI.BeginChangeCheck();
        {
            var pProperty_bIsCurrent = pSerializeObject.FindProperty($"{nameof(LogWrapperEditorSetting.bIsCurrent)}");
            EditorGUILayout.PropertyField(pProperty_bIsCurrent, true);

            var pProperty_arrDebugFilter = pSerializeObject.FindProperty($"{nameof(LogWrapperEditorSetting.arrLogType)}");
            EditorGUILayout.PropertyField(pProperty_arrDebugFilter, true);

            var pProperty_arrTest = pSerializeObject.FindProperty($"{nameof(LogWrapperEditorSetting.arrBranch)}");
            for (int i = 0; i < pProperty_arrTest.arraySize; i++)
            {
                SerializedProperty pProperty = pProperty_arrTest.GetArrayElementAtIndex(i);
                SerializedProperty pPropertySetting = pProperty.FindPropertyRelative($"{nameof(LogFilter_PerBranch.pEditorSetting)}");
                if (pPropertySetting.objectReferenceValue == null)
                {
                    pPropertySetting.objectReferenceValue = pEditorSetting;
                    bIsSet_EditorSetting = true;
                }
            }

            EditorGUILayout.PropertyField(pProperty_arrTest, true);
        }

        if (bIsSet_EditorSetting || EditorGUI.EndChangeCheck())
        {
            pSerializeObject.ApplyModifiedProperties();
        }
    }
#endif

}

#if UNITY_EDITOR

[CanEditMultipleObjects]
[CustomEditor(typeof(ScriptableObject), true)]
public class DebugWrapperSetting_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        LogWrapperEditorSetting.DoDrawEditorGUI(serializedObject, target as LogWrapperEditorSetting);
    }
}

// PropertyDrawer 안에 PropertyDrawer가 있으면 ReorderableList가 Select가 안됨;
// https://stackoverflow.com/questions/54516221/how-to-select-elements-in-nested-reorderablelist-in-a-customeditor
[CustomPropertyDrawer(typeof(LogWrapperEditorSetting), true)]
public class DebugWrapperSetting_PropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PropertyField(position, property, label);
        position.y += 20f;

        EditorGUI.BeginProperty(position, label, property);
        {
            if (property.objectReferenceValue != null)
            {
                SerializedObject pSOThis = new SerializedObject(property.objectReferenceValue);
                LogWrapperEditorSetting.DoDrawEditorGUI(pSOThis, property.objectReferenceValue as LogWrapperEditorSetting);
            }
        }
        EditorGUI.EndProperty();
    }

}

#endif
