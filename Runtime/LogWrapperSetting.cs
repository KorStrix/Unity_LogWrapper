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
public class LogWrapperSetting : ScriptableObject
{
    public static LogWrapperSetting pCurrentSetting
    {
        get
        {
            //if (_pCurrentSetting == null)
            {
                LogWrapperSetting[] arrSetting = Resources.LoadAll<LogWrapperSetting>("");

                int iCurrentSettingCount = arrSetting.Count(p => p.bIsCurrent);
                if (iCurrentSettingCount > 1)
                {
                    UnityEngine.Debug.LogWarning($"{nameof(LogWrapperSetting)} - iCurrentSettingCount({iCurrentSettingCount}) > 1");
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
                            _pCurrentSetting = LogWrapperUtility.CreateAsset<LogWrapperSetting>();
                            UnityEngine.Debug.Log($"{nameof(LogWrapperSetting)} is null / auto create default setting", _pCurrentSetting);
                        }
                        else
                        {
                            _pCurrentSetting = CreateInstance<LogWrapperSetting>();
                            UnityEngine.Debug.LogWarning($"{nameof(LogWrapperSetting)} is null / auto create default setting");
                        }
                    }

                    _pCurrentSetting.bIsCurrent = true;
                }
            }

            return _pCurrentSetting;
        }
    }

    static LogWrapperSetting _pCurrentSetting;

    public bool bIsCurrent;
    public string strTypeName;
    public string strCSExportPath;
    public CustomLogType[] arrLogType = new CustomLogType[0];

    // 여기에 브렌치별 Log 필터 정보도 넣어야 할듯?
    public LogFilter_PerBranch[] arrBranch;


#if UNITY_EDITOR
    public static void DoDrawEditorGUI(SerializedObject pSerializeObject, LogWrapperSetting pSetting)
    {
        pSerializeObject.Update();

        bool bIsSet_EditorSetting = false;

        EditorGUI.BeginChangeCheck();
        {
            EditorGUILayout.PropertyField(pSerializeObject.FindProperty($"{nameof(LogWrapperSetting.bIsCurrent)}"));

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.PropertyField(pSerializeObject.FindProperty($"{nameof(LogWrapperSetting.strTypeName)}"), new GUIContent("TypeName"));
                EditorGUILayout.PropertyField(pSerializeObject.FindProperty($"{nameof(LogWrapperSetting.strCSExportPath)}"), new GUIContent("CSExportPath"));
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(pSerializeObject.FindProperty($"{nameof(LogWrapperSetting.arrLogType)}"), true);

            var pProperty_arrTest = pSerializeObject.FindProperty($"{nameof(LogWrapperSetting.arrBranch)}");
            for (int i = 0; i < pProperty_arrTest.arraySize; i++)
            {
                SerializedProperty pProperty = pProperty_arrTest.GetArrayElementAtIndex(i);
                SerializedProperty pPropertySetting = pProperty.FindPropertyRelative($"{nameof(LogFilter_PerBranch.pSetting)}");
                if (pPropertySetting.objectReferenceValue == null)
                {
                    pPropertySetting.objectReferenceValue = pSetting;
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
[CustomEditor(typeof(LogWrapperSetting), true)]
public class LogWrapperSetting_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        LogWrapperSetting.DoDrawEditorGUI(serializedObject, target as LogWrapperSetting);
    }
}

// PropertyDrawer 안에 PropertyDrawer가 있으면 ReorderableList가 Select가 안됨;
// https://stackoverflow.com/questions/54516221/how-to-select-elements-in-nested-reorderablelist-in-a-customeditor
[CustomPropertyDrawer(typeof(LogWrapperSetting), true)]
public class LogWrapperSetting_PropertyDrawer : PropertyDrawer
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
                LogWrapperSetting.DoDrawEditorGUI(pSOThis, property.objectReferenceValue as LogWrapperSetting);
            }
        }
        EditorGUI.EndProperty();
    }

}

#endif
