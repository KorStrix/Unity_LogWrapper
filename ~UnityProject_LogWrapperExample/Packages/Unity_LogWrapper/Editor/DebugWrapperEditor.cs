#region Header
/*	============================================
 *	Author   			    : Strix
 *	Initial Creation Date 	: 2020-03-15
 *	Summary 		        : 
 *
 *
 * 참고한 코드
 * - ReorderableList - https://unityindepth.tistory.com/56
 *  Template 		        : For Unity Editor V1
   ============================================ */
#endregion Header

using UnityEngine;
using UnityEditor;
using Wrapper;
using System.Linq;
using System.Collections.Generic;

using Debug = UnityEngine.Debug;

/// <summary>
/// 
/// </summary>
public class DebugWrapperEditor : EditorWindow
{
    /* const & readonly declaration             */

    const string const_strPlayerPefs_SaveKey = nameof(CustomLogType_Enable);

    /* enum & struct declaration                */


    /* public - Field declaration               */

    public DebugWrapperEditorSetting pEditorSetting;
    public CustomLogType_Enable[] pLogTypeEnableArray = null;

    /* protected & private - Field declaration  */

    // ========================================================================== //

    /* public - [Do~Something] Function 	        */

    [MenuItem("Tools/DebugWrapper Editor")]
    static void ShowWindow()
    {
        DebugWrapperEditor pWindow = (DebugWrapperEditor)GetWindow(typeof(DebugWrapperEditor), false);

        pWindow.minSize = new Vector2(300, 300);
        pWindow.Show();
    }

    // ========================================================================== //

    /* protected - [Override & Unity API]       */

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Debug Wrapper Editor", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        EditorGUILayout.Space();


        SerializedObject pSO = new SerializedObject(this);

        EditorGUILayout.LabelField("Editor Setting", EditorStyles.boldLabel);
        Draw_EditorSetting(pSO);
        
        

        EditorGUILayout.Space();
        if (pEditorSetting != null)
        {
            Draw_CSExportButton();
            EditorGUILayout.Space();
            EditorGUILayout.Space();


            EditorGUILayout.LabelField("Local Editor Setting", EditorStyles.boldLabel);
            // Draw_LocalEditor_EnableSetting(pSO);
        }
        else
            EditorGUILayout.LabelField("Require Editor Setting");

        if (GUI.changed)
        {
            pSO.ApplyModifiedProperties();
            EditorUtility.SetDirty(this);
        }
    }
    /* protected - [abstract & virtual]         */


    // ========================================================================== //

    #region Private

    private void Get_LogTypeEnable_FromPlayerPrefs()
    {
        if (pLogTypeEnableArray == null)
            pLogTypeEnableArray = new CustomLogType_Enable[0];

        bool bIsRequireUpdate_LogTypeEnableArray = CustomLogType.Load_FromPlayerPrefs(const_strPlayerPefs_SaveKey, ref pLogTypeEnableArray) == false;
        if (bIsRequireUpdate_LogTypeEnableArray)
        {
            CustomLogType_Enable.DoMatch_LogTypeEnableArray(pEditorSetting, ref pLogTypeEnableArray);
            CustomLogType.Save_ToPlayerPrefs(const_strPlayerPefs_SaveKey, pLogTypeEnableArray);
        }
    }

    Vector2 _vecScrollPos;

    private void Draw_EditorSetting(SerializedObject pSO)
    {
        if (pEditorSetting == null)
        {
            if (GUILayout.Button("Create Setting File And Set"))
            {
                pEditorSetting = CreateAsset<DebugWrapperEditorSetting>();
                Debug.Log("Create And Set");
            }
        }
        else
        {
            if (GUILayout.Button("Create New Setting File"))
            {
                CreateAsset<DebugWrapperEditorSetting>();
                Debug.Log("Create New");
            }
        }

        _vecScrollPos = EditorGUILayout.BeginScrollView(_vecScrollPos, GUILayout.Height(300f));
        {
            SerializedProperty pProperty = pSO.FindProperty($"{nameof(pEditorSetting)}");
            EditorGUILayout.PropertyField(pProperty);
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.Separator();
    }


    private void Draw_LocalEditor_EnableSetting(SerializedObject pSO)
    {
        Get_LogTypeEnable_FromPlayerPrefs();

        SerializedProperty pProperty = pSO.FindProperty($"{nameof(pLogTypeEnableArray)}");
        EditorGUILayout.PropertyField(pProperty);

        if (GUI.changed)
        {
            CustomLogType.Save_ToPlayerPrefs(const_strPlayerPefs_SaveKey, pLogTypeEnableArray);
        }
    }

    private void Draw_CSExportButton()
    {
        const string strExportCS = "Export CS";

        EditorGUILayout.LabelField($"{strExportCS} Path (Assets/*.cs)");
        pEditorSetting.strCSExportPath = EditorGUILayout.TextField(pEditorSetting.strCSExportPath);

        if (GUILayout.Button(strExportCS))
        {
            if (string.IsNullOrEmpty(pEditorSetting.strCSExportPath))
            {
                Debug.LogError($"{strExportCS} - string.IsNullOrEmpty(_strCSExportPath)");
                return;
            }

            CustomCodedom pCodeDom = new CustomCodedom();
            foreach (var pFilter in pEditorSetting.arrLogType)
                pCodeDom.DoAddClass(pFilter);
            pCodeDom.DoExportCS($"{Application.dataPath}/{pEditorSetting.strCSExportPath}");

            AssetDatabase.Refresh();
            Debug.Log($"{strExportCS} Complete");
        }
    }

    #endregion Private

    #region Tool

    public static T CreateAsset<T>() where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath("Assets/New " + typeof(T) + ".asset");

        AssetDatabase.CreateAsset(asset, assetPathAndName);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;

        return asset;
    }

    #endregion Tool
}
