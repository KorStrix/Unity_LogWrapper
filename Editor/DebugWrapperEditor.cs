#region Header
/*	============================================
 *	Author   			    : Strix
 *	Initial Creation Date 	: 2020-03-15
 *	Summary 		        : 
 *
 *  Template 		        : For Unity Editor V1
   ============================================ */
#endregion Header

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

/// <summary>
/// 
/// </summary>
public class DebugWrapperEditor : EditorWindow
{
    /* const & readonly declaration             */

    const string strSaveKey = nameof(DebugWrapperSetting);

    /* enum & struct declaration                */

    [System.Serializable]
    public class DebugWrapperSetting
    {
        public Wrapper.DebugFilter[] arrDebugFilter = new Wrapper.DebugFilter[0];
    }

    /* public - Field declaration               */

    public DebugWrapperSetting pDebugInfo;

    /* protected & private - Field declaration  */


    // ========================================================================== //

    /* public - [Do~Something] Function 	        */

    [MenuItem("Tools/DebugWrapper Editor")]
    static void Init()
    {
        DebugWrapperEditor pWindow = (DebugWrapperEditor)GetWindow(typeof(DebugWrapperEditor), false);

        pWindow.minSize = new Vector2(600, 300);
        pWindow.Show();
    }

    // ========================================================================== //

    /* protected - [Override & Unity API]       */

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Debug Wrapper Editor", EditorStyles.boldLabel);
        EditorGUILayout.Separator();


        if (pDebugInfo == null)
        {
            pDebugInfo = new DebugWrapperSetting();
            if (Load_FromEditorPrefs(strSaveKey, ref pDebugInfo) == false)
                Save_ToEditorPrefs(strSaveKey, pDebugInfo);
        }


        SerializedObject pSO = new SerializedObject(this);
        EditorGUILayout.PropertyField(pSO.FindProperty($"{nameof(pDebugInfo)}"));

        if (GUI.changed)
        {
            Save_ToEditorPrefs(strSaveKey, pDebugInfo);
        }
    }


    /* protected - [abstract & virtual]         */


    // ========================================================================== //

    #region Private

    public static void Save_ToEditorPrefs(string strKey, object pSerializeObject)
    {
        string strJsonText = JsonUtility.ToJson(pSerializeObject);
        EditorPrefs.SetString(strKey, strJsonText);
    }

    public static bool Load_FromEditorPrefs<T>(string strKey, ref T pLoadObject_NotNull, System.Action<string> OnError = null)
    {
        if (EditorPrefs.HasKey(strKey) == false)
        {
            OnError?.Invoke($"{nameof(Load_FromEditorPrefs)} - PlayerPrefs.HasKey({strKey}) == false");

            return false;
        }

        string strJson = EditorPrefs.GetString(strKey);
        try
        {
            JsonUtility.FromJsonOverwrite(strJson, pLoadObject_NotNull);
        }
        catch (System.Exception e)
        {
            OnError?.Invoke($"{nameof(Load_FromEditorPrefs)} - FromJsonOverwrite Fail - {strKey} Value : \n{strJson}\n{e}");

            return false;
        }

        return true;
    }


    #endregion Private
}


[CustomPropertyDrawer(typeof(DebugWrapperEditor.DebugWrapperSetting))]
public class DebugWrapperSettingDrawer : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        property.serializedObject.Update();

        ArrayGUI(property.FindPropertyRelative("arrDebugFilter"));

        property.serializedObject.ApplyModifiedProperties();
    }

    private void ArrayGUI(SerializedProperty commands)
    {
        GUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Add"))
                commands.arraySize++;
            if (GUILayout.Button("Remove") && commands.arraySize > 0)
                commands.arraySize--;
        }
        GUILayout.EndHorizontal();

        for (int i = 0; i < commands.arraySize; i++)
            EditorGUILayout.PropertyField(commands.GetArrayElementAtIndex(i), new GUIContent("Command " + (i + 1).ToString() + ": "));
    }
}

[CustomPropertyDrawer(typeof(Wrapper.DebugFilter))]
public class DebugFilterDrawer : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Wrapper.DebugFilter sFilter = null;

        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Calculate rects
        var amountRect = new Rect(position.x, position.y, 30, position.height);
        var unitRect = new Rect(position.x + 35, position.y, 50, position.height);

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        // EditorGUI.PropertyField(amountRect, property.FindPropertyRelative(nameof(sFilter.pFilterFlag)), GUIContent.none);


        SerializedProperty pProperty_ColorHexCode = property.FindPropertyRelative(nameof(sFilter.strColorHexCode));
        EditorGUI.PropertyField(unitRect, pProperty_ColorHexCode, GUIContent.none);

        EditorGUI.EndProperty();
    }
}