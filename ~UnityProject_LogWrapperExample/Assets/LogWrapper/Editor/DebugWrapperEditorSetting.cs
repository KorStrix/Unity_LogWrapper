using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

[Serializable]
public class DebugWrapperEditorSetting : ScriptableObject
{
    public string strCSExportPath;
    public Wrapper.CustomLogType[] arrDebugFilter = new Wrapper.CustomLogType[0];

    // 여기에 브렌치별 Log 필터 정보도 넣어야 할듯?
    public LogFilter_PerBranch arrTest;
}

[Serializable]
public class LogFilter_PerBranch
{
    public string strBranchName;
    // public CustomLogType_Enable[] arrLogTypeEnable;
    public CustomLogType_EnableArray arrLogTypeEnable;
}


#if UNITY_EDITOR

// PropertyDrawer 안에 PropertyDrawer가 있으면 ReorderableList가 Select가 안됨;
// https://stackoverflow.com/questions/54516221/how-to-select-elements-in-nested-reorderablelist-in-a-customeditor
[CustomPropertyDrawer(typeof(DebugWrapperEditorSetting))]
public class DebugWrapperSettingDrawer : PropertyDrawer
{
    private Dictionary<string, ReorderableList> innerListDict = new Dictionary<string, ReorderableList>();
    private SerializedObject _pSOThis;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PropertyField(position, property, label);

        EditorGUI.BeginProperty(position, label, property);
        {
            if (property.objectReferenceValue != null)
            {
                if(_pSOThis == null)
                    _pSOThis = new SerializedObject(property.objectReferenceValue);
                _pSOThis.Update();

                EditorGUI.BeginChangeCheck();
                {
                    var pProperty_arrDebugFilter = _pSOThis.FindProperty($"{nameof(DebugWrapperEditorSetting.arrDebugFilter)}");
                    DrawLogFilterArray(_pSOThis, pProperty_arrDebugFilter);

                    var pProperty_arrTest = _pSOThis.FindProperty($"{nameof(DebugWrapperEditorSetting.arrTest)}");
                    EditorGUILayout.PropertyField(pProperty_arrTest);
                    // DrawLogFilterArray(_pSOThis, pProperty_arrTest);
                }
                if (EditorGUI.EndChangeCheck())
                {
                    foreach (var pList in innerListDict.Values)
                    {
                        if (pList != null &&
                            pList.serializedProperty.propertyType == SerializedPropertyType.ObjectReference &&
                            pList.serializedProperty.objectReferenceValue != null)
                            EditorUtility.SetDirty(pList.serializedProperty.objectReferenceValue);
                    }

                    _pSOThis.ApplyModifiedProperties();
                }
            }
        }
        EditorGUI.EndProperty();
    }

    private void DrawLogFilterArray(SerializedObject pSO, SerializedProperty propertyArray)
    {
        string listKey = propertyArray.propertyPath;

        if (innerListDict.TryGetValue(listKey, out ReorderableList pCurrentList) == false ||
            pCurrentList == null)
        {
            pCurrentList = new ReorderableList(pSO, propertyArray);

            pCurrentList.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, propertyArray.displayName);

            pCurrentList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                var element = pCurrentList.serializedProperty.GetArrayElementAtIndex(index);

                // 밑에 코드가 없으면 뭔가 삐뚤어져있음
                rect.height -= 4;
                rect.y += 2;

                EditorGUI.PropertyField(rect, element);
            };

            innerListDict[listKey] = pCurrentList;
        }

        pCurrentList.DoLayoutList();
    }
}

[CustomPropertyDrawer(typeof(LogFilter_PerBranch))]
public class LogFilter_PerBranchDrawer : PropertyDrawer
{
    private const float fLabelWidth_strBranchName = 50f;
    private const float fLabelOffset = 5f;


    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        {
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            SerializedProperty pProperty_strBranchName = property.FindPropertyRelative(nameof(LogFilter_PerBranch.strBranchName));
            EditorGUI.PropertyField(CalculateRect(ref position, fLabelWidth_strBranchName, fLabelOffset), pProperty_strBranchName, GUIContent.none);

            SerializedProperty pProperty_arrLogTypeEnable = property.FindPropertyRelative(nameof(LogFilter_PerBranch.arrLogTypeEnable));
            EditorGUI.PropertyField(CalculateRect(ref position, fLabelWidth_strBranchName, fLabelOffset), pProperty_arrLogTypeEnable, GUIContent.none);

            label.text = $"{pProperty_strBranchName.stringValue}";
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
