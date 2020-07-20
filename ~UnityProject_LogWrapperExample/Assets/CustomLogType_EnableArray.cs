using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

[Serializable]
public class CustomLogType_EnableArray : ScriptableObject
{
    public CustomLogType_Enable[] arrLogEnable = new CustomLogType_Enable[0];
}

[Serializable]
public class CustomLogType_Enable
{
    public string strCustomLogName;
    public bool bEnable = true;

    public CustomLogType_Enable(string strCustomLogName)
    {
        this.strCustomLogName = strCustomLogName;
    }
}

#if UNITY_EDITOR

// PropertyDrawer 안에 PropertyDrawer가 있으면 ReorderableList가 Select가 안됨;
// https://stackoverflow.com/questions/54516221/how-to-select-elements-in-nested-reorderablelist-in-a-customeditor
[CustomPropertyDrawer(typeof(CustomLogType_EnableArray))]
public class CustomLogType_EnableArrayDrawer : PropertyDrawer
{
    private Dictionary<string, ReorderableList> innerListDict = new Dictionary<string, ReorderableList>();
    private SerializedObject _pSOThis;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // EditorGUI.PropertyField(position, property, label);
        EditorGUI.BeginProperty(position, label, property);
        {
            if (property.objectReferenceValue != null)
            {
                if (_pSOThis == null)
                    _pSOThis = new SerializedObject(property.objectReferenceValue);
                _pSOThis.Update();

                EditorGUI.BeginChangeCheck();

                var propertyArray = _pSOThis.FindProperty($"{nameof(CustomLogType_EnableArray.arrLogEnable)}");
                DrawLogFilterArray(_pSOThis, propertyArray);

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

            pCurrentList.draggable = false;
            pCurrentList.displayAdd = false;
            pCurrentList.displayRemove = false;

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

        foreach (var pList in innerListDict.Values)
            pList.DoLayoutList();
    }
}


[CustomPropertyDrawer(typeof(CustomLogType_Enable))]
public class CustomLogType_EnableDrawer : PropertyDrawer
{
    private const float fLabelWidth_bEnable = 50f;
    private const float fLabelOffset = 5f;


    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        {
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            SerializedProperty pProperty_bEnable = property.FindPropertyRelative(nameof(CustomLogType_Enable.bEnable));
            EditorGUI.PropertyField(CalculateRect(ref position, fLabelWidth_bEnable, fLabelOffset), pProperty_bEnable, GUIContent.none);

            SerializedProperty pProperty_strCustomLogName = property.FindPropertyRelative(nameof(CustomLogType_Enable.strCustomLogName));
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
