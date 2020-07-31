using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class DebugWrapperEditorSetting : ScriptableObject
{
    public string strCSExportPath;
    public CLogType[] arrLogType = new CLogType[0];

    // 여기에 브렌치별 Log 필터 정보도 넣어야 할듯?
    public LogFilter_PerBranch[] arrBranch;


    public static void DoDrawEditorGUI(SerializedObject pSerializeObject, DebugWrapperEditorSetting pEditorSetting)
    {
        pSerializeObject.Update();

        bool bIsSet_EditorSetting = false;

        EditorGUI.BeginChangeCheck();
        {
            var pProperty_arrDebugFilter = pSerializeObject.FindProperty($"{nameof(DebugWrapperEditorSetting.arrLogType)}");
            EditorGUILayout.PropertyField(pProperty_arrDebugFilter, true);

            var pProperty_arrTest = pSerializeObject.FindProperty($"{nameof(DebugWrapperEditorSetting.arrBranch)}");
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
}

#if UNITY_EDITOR

[CanEditMultipleObjects]
[CustomEditor(typeof(ScriptableObject), true)]
public class DebugWrapperSetting_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DebugWrapperEditorSetting.DoDrawEditorGUI(serializedObject, target as DebugWrapperEditorSetting);
    }
}

// PropertyDrawer 안에 PropertyDrawer가 있으면 ReorderableList가 Select가 안됨;
// https://stackoverflow.com/questions/54516221/how-to-select-elements-in-nested-reorderablelist-in-a-customeditor
[CustomPropertyDrawer(typeof(DebugWrapperEditorSetting), true)]
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
                DebugWrapperEditorSetting.DoDrawEditorGUI(pSOThis, property.objectReferenceValue as DebugWrapperEditorSetting);
            }
        }
        EditorGUI.EndProperty();
    }

}

#endif
