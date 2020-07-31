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

/// <summary>
/// 
/// </summary>
public class DebugWrapperEditor : EditorWindow
{
    /* const & readonly declaration             */


    /* enum & struct declaration                */


    /* public - Field declaration               */

    public DebugWrapperEditorSetting pEditorSetting;
    public LogFilter_PerBranch pLocalBranch;

    /* protected & private - Field declaration  */

    // ========================================================================== //

    /* public - [Do~Something] Function 	        */

    [MenuItem("Tools/DebugWrapper Editor")]
    static void ShowWindow()
    {
        DebugWrapperEditor pWindow = (DebugWrapperEditor)GetWindow(typeof(DebugWrapperEditor), false);

        pWindow.minSize = new Vector2(300, 500);
        pWindow.Show();
    }

    // ========================================================================== //

    /* protected - [Override & Unity API]       */

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Debug Wrapper Editor", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("This tool is for managing log filters by DefineSymbol after building Debog.Log on local PC.", MessageType.Info);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Work sequence", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox(@"0. Create or set EditorSetting File.

1. Create a LogFilter.
In addition to the LogType name (required)
You can adjust Comment, number(for Filter), print color, etc.

2. Set which LogType to output for each branch.

3. The script is automatically generated through ExportCS.
This is necessary when Nos. 1 and 2 are modified.

4. Set which LogFilter to output from other LocalPC.
(I used PlayerPrefs.)", MessageType.Info);

        EditorGUILayout.Space();
        EditorGUILayout.Space();


        SerializedObject pSO = new SerializedObject(this);

        EditorGUILayout.LabelField("[0~3]. Editor Setting", EditorStyles.boldLabel);
        Draw_EditorSetting(pSO);



        EditorGUILayout.Space();
        if (pEditorSetting != null)
        {
            Draw_CSExportButton();
            EditorGUILayout.Space();
            EditorGUILayout.Space();


            EditorGUILayout.LabelField("[4]. Local Editor Setting", EditorStyles.boldLabel);
            Draw_LocalEditor_EnableSetting(pSO);
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

    private void Get_LogTypeEnable_FromPlayerPrefs(SerializedObject pSO_this)
    {
        LogFilter_PerBranch pCurrentBranch = LogFilter_PerBranch.Get_LogTypeEnable_FromPlayerPrefs(out bool bIsSave);
        if (bIsSave || ReferenceEquals(pLocalBranch, pCurrentBranch) == false)
            pLocalBranch = pCurrentBranch;

        if (CustomLogType_Enable.DoMatch_LogTypeEnableArray(pEditorSetting, ref pLocalBranch.arrLogTypeEnable))
        {
            LogWrapperUtility.Save_ToPlayerPrefs(LogFilter_PerBranch.const_strPlayerPefs_SaveKey, pLocalBranch);
            bIsSave = true;
        }

        if (pLocalBranch.pEditorSetting != pEditorSetting)
        {
            pLocalBranch.pEditorSetting = pEditorSetting;
            bIsSave = true;
        }

        if (bIsSave)
        {
            pSO_this.ApplyModifiedProperties();
            EditorUtility.SetDirty(pSO_this.targetObject);
        }
    }

    Vector2 _vecScrollPos_EditorSetting;

    private void Draw_EditorSetting(SerializedObject pSO)
    {
        if (pEditorSetting == null)
        {
            if (GUILayout.Button("Create Setting File And Set"))
            {
                pEditorSetting = CreateAsset<DebugWrapperEditorSetting>();
                UnityEngine.Debug.Log("Create And Set");
            }
        }
        else
        {
            if (GUILayout.Button("Create New Setting File"))
            {
                CreateAsset<DebugWrapperEditorSetting>();
                UnityEngine.Debug.Log("Create New");
            }
        }

        _vecScrollPos_EditorSetting = EditorGUILayout.BeginScrollView(_vecScrollPos_EditorSetting, GUILayout.Height(300f));
        {
            SerializedProperty pProperty = pSO.FindProperty($"{nameof(pEditorSetting)}");
            EditorGUILayout.PropertyField(pProperty);
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.Separator();
    }


    Vector2 _vecScrollPos_Local;

    private void Draw_LocalEditor_EnableSetting(SerializedObject pSO)
    {
        Get_LogTypeEnable_FromPlayerPrefs(pSO);

        SerializedProperty pProperty = pSO.FindProperty($"{nameof(pLocalBranch)}");
        _vecScrollPos_Local = EditorGUILayout.BeginScrollView(_vecScrollPos_Local, GUILayout.Height(300f));
        {
            EditorGUILayout.PropertyField(pProperty, true);
        }
        EditorGUILayout.EndScrollView();

        if (GUI.changed)
        {
            pSO.ApplyModifiedProperties();
            EditorUtility.SetDirty(pSO.targetObject);
            LogWrapperUtility.Save_ToPlayerPrefs(LogFilter_PerBranch.const_strPlayerPefs_SaveKey, pLocalBranch);
        }
    }

    private void Draw_CSExportButton()
    {
        const string strExportCS = "Export CS";

        EditorGUILayout.LabelField($"LogFilter Type Name");
        pEditorSetting.strTypeName = EditorGUILayout.TextField(pEditorSetting.strTypeName);

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField($"{strExportCS} Path (Assets/*.cs)");
            pEditorSetting.strCSExportPath = EditorGUILayout.TextField(pEditorSetting.strCSExportPath);

            if (GUILayout.Button(strExportCS))
            {
                if (string.IsNullOrEmpty(pEditorSetting.strTypeName))
                {
                    UnityEngine.Debug.LogError($"{strExportCS} - string.IsNullOrEmpty({nameof(pEditorSetting.strTypeName)})");
                    return;
                }

                if (string.IsNullOrEmpty(pEditorSetting.strCSExportPath))
                {
                    UnityEngine.Debug.LogError($"{strExportCS} - string.IsNullOrEmpty({nameof(pEditorSetting.strCSExportPath)})");
                    return;
                }

                ExportCS(strExportCS);
            }
        }
        EditorGUILayout.EndHorizontal();

    }

    private void ExportCS(string strExportCS)
    {
        CustomCodedom pCodeDom = new CustomCodedom();
        foreach (var pFilter in pEditorSetting.arrLogType)
            pCodeDom.DoAddClass(pFilter);

        foreach (var pBranch in pEditorSetting.arrBranch)
            pCodeDom.DoAddBranch(pBranch);

        pCodeDom.DoExportCS(pEditorSetting.strTypeName, $"{Application.dataPath}/{pEditorSetting.strCSExportPath}");

        AssetDatabase.Refresh();
        UnityEngine.Debug.Log($"{strExportCS} Complete");
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
