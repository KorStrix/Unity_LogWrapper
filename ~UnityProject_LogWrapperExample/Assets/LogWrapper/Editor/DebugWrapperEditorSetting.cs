using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

[Serializable]
public class DebugWrapperEditorSetting : ScriptableObject
{
    public Wrapper.LogFilterFlag[] arrDebugFilter = new Wrapper.LogFilterFlag[0];
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

                var propertyArray = _pSOThis.FindProperty($"{nameof(DebugWrapperEditorSetting.arrDebugFilter)}");
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
#endif
