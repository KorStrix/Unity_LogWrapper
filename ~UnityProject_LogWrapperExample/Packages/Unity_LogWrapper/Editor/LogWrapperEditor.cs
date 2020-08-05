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

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Wrapper;

/// <summary>
/// 
/// </summary>
public class LogWrapperEditor : EditorWindow
{
    /* const & readonly declaration             */


    /* enum & struct declaration                */


    /* public - Field declaration               */

    public LogWrapperSetting pSetting;
    public LogFilter_PerBranch pLocalBranch;

    /* protected & private - Field declaration  */

    private bool _bIsShow_LogSetting;
    private bool _bIsShow_WorkSequence;

    // ========================================================================== //

    /* public - [Do~Something] Function 	        */

    [MenuItem("Tools/LogWrapper Editor")]
    static void ShowWindow()
    {
        LogWrapperEditor pWindow = (LogWrapperEditor)GetWindow(typeof(LogWrapperEditor), false);

        pWindow.pSetting = LogWrapperSetting.pCurrentSetting;
        pWindow.minSize = new Vector2(300, 500);
        pWindow.Show();
    }

    // ========================================================================== //

    /* protected - [Override & Unity API]       */

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Log Wrapper Editor", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("This tool is for managing log filters by DefineSymbol after building Debog.Log on local PC.", MessageType.Info);
        EditorGUILayout.Space();

        Draw_WorkSequence();


        SerializedObject pSO = new SerializedObject(this);

        EditorGUILayout.LabelField("[0~3]. Editor Setting", EditorStyles.boldLabel);
        Draw_EditorSetting(pSO);



        EditorGUILayout.Space();
        if (pSetting != null)
        {
            if(IsShowSetting(nameof(_bIsShow_LogSetting)))
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

            // Editor에서 플레이중인 경우
            if (Application.isPlaying)
                SetCurrentLogFilter();
        }
    }

    /* protected - [abstract & virtual]         */


    // ========================================================================== //

    #region Private

    private static void SetCurrentLogFilter()
    {
        List<CustomDebug.ICustomLogType> list = new List<CustomDebug.ICustomLogType>();

        LogFilter_PerBranch pLocalBranch = LogFilter_PerBranch.Get_LogTypeEnable_FromPlayerPrefs(out bool bIsChange);
        if (bIsChange)
        {
            UnityEngine.Debug.LogError($"Get LogTypeEnable FromPlayerPrefs Fail");
            return;
        }

        list.AddRange(pLocalBranch.GetEnableLogType());

        Wrapper.Debug.Init_PrintLog_FilterFlag(list.ToArray());
    }

    private void Draw_WorkSequence()
    {
        EditorGUILayout.LabelField("Work sequence", EditorStyles.boldLabel);

        if (IsShowSetting(nameof(_bIsShow_WorkSequence)))
        {
            if (GUILayout.Button("Hide WorkSequence"))
            {
                SaveSetting_Hide(nameof(_bIsShow_WorkSequence), ref _bIsShow_WorkSequence);
            }

            EditorGUILayout.HelpBox(@"0. Create or set EditorSetting File.

1. Create a LogFilter.
In addition to the LogType name (required)
You can adjust Comment, number(for Filter), print color, etc.

2. Set which LogType to output for each branch.

3. The script is automatically generated through ExportCS.
This is necessary when Nos. 1 and 2 are modified.

4. Set which LogFilter to output from other LocalPC.
(I used PlayerPrefs.)", MessageType.Info);
        }
        else
        {
            if (GUILayout.Button("Show WorkSequence"))
            {
                SaveSetting_Show(nameof(_bIsShow_WorkSequence), ref _bIsShow_WorkSequence);
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();
    }

    private void Get_LogTypeEnable_FromPlayerPrefs(SerializedObject pSO_this)
    {
        LogFilter_PerBranch pCurrentBranch = LogFilter_PerBranch.Get_LogTypeEnable_FromPlayerPrefs(out bool bIsSave);
        if (bIsSave || pLocalBranch == null || pLocalBranch.arrLogTypeEnable == null || pLocalBranch.arrLogTypeEnable.Length != pCurrentBranch.arrLogTypeEnable.Length)
        {
            pLocalBranch = pCurrentBranch;
            bIsSave = true;
        }

        bIsSave = CustomLogType_Enable.DoMatch_LogTypeEnableArray(pSetting, ref pLocalBranch.arrLogTypeEnable);

        if (pLocalBranch.pSetting != pSetting)
        {
            pLocalBranch.pSetting = pSetting;
            bIsSave = true;
        }

        if (bIsSave)
        {
            SaveSO(pSO_this);
            LogWrapperUtility.Save_ToPlayerPrefs(LogFilter_PerBranch.const_strPlayerPrefs_SaveKey, pLocalBranch);
        }
    }

    Vector2 _vecScrollPos_EditorSetting;

    private void Draw_EditorSetting(SerializedObject pSO)
    {
        if (IsShowSetting(nameof(_bIsShow_LogSetting)))
        {
            if (GUILayout.Button("Hide LogSetting"))
            {
                SaveSetting_Hide(nameof(_bIsShow_LogSetting), ref _bIsShow_LogSetting);
            }

            if (pSetting == null)
            {
                if (GUILayout.Button("Create Setting File And Set"))
                {
                    pSetting = LogWrapperUtility.CreateAsset<LogWrapperSetting>();
                    UnityEngine.Debug.Log("Create And Set");
                }
            }
            else
            {
                if (GUILayout.Button("Create New Setting File"))
                {
                    LogWrapperUtility.CreateAsset<LogWrapperSetting>();
                    UnityEngine.Debug.Log("Create New");
                }
            }

            _vecScrollPos_EditorSetting = EditorGUILayout.BeginScrollView(_vecScrollPos_EditorSetting, GUILayout.Height(300f));
            {
                SerializedProperty pProperty = pSO.FindProperty($"{nameof(pSetting)}");
                EditorGUI.BeginChangeCheck();
                {
                    EditorGUILayout.PropertyField(pProperty);
                }
                if (EditorGUI.EndChangeCheck())
                    SaveSO(pProperty.serializedObject);
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.Separator();
        }
        else
        {
            if (GUILayout.Button("Show LogSetting"))
            {
                SaveSetting_Show(nameof(_bIsShow_LogSetting), ref _bIsShow_LogSetting);
            }
        }
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
            SaveSO(pSO);
            LogWrapperUtility.Save_ToPlayerPrefs(LogFilter_PerBranch.const_strPlayerPrefs_SaveKey, pLocalBranch);
        }
    }

    private void Draw_CSExportButton()
    {
        const string strExportCS = "Export CS";

        EditorGUILayout.LabelField($"LogFilter Type Name");
        pSetting.strTypeName = EditorGUILayout.TextField(pSetting.strTypeName);

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField($"{strExportCS} Path (Assets/*.cs)");
            pSetting.strCSExportPath = EditorGUILayout.TextField(pSetting.strCSExportPath);

            if (GUILayout.Button(strExportCS))
            {
                if (string.IsNullOrEmpty(pSetting.strTypeName))
                {
                    UnityEngine.Debug.LogError($"{strExportCS} - string.IsNullOrEmpty({nameof(pSetting.strTypeName)})");
                    return;
                }

                if (string.IsNullOrEmpty(pSetting.strCSExportPath))
                {
                    UnityEngine.Debug.LogError($"{strExportCS} - string.IsNullOrEmpty({nameof(pSetting.strCSExportPath)})");
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
        foreach (var pFilter in pSetting.arrLogType)
            pCodeDom.DoAddClass(pFilter);

        foreach (var pBranch in pSetting.arrBranch)
            pCodeDom.DoAddBranch(pBranch);

        pCodeDom.DoExportCS(pSetting.strTypeName, $"{Application.dataPath}/{pSetting.strCSExportPath}");

        AssetDatabase.Refresh();
        UnityEngine.Debug.Log($"{strExportCS} Complete");
    }

    private bool IsShowSetting(string strSaveKey)
    {
        return PlayerPrefs.GetInt(strSaveKey) == 0;
    }

    private void SaveSetting_Show(string strSaveKey, ref bool bValue)
    {
        bValue = true;
        PlayerPrefs.SetInt(strSaveKey, 0);
    }

    private void SaveSetting_Hide(string strSaveKey, ref bool bValue)
    {
        bValue = false;
        PlayerPrefs.SetInt(strSaveKey, 1);
    }

    private static void SaveSO(SerializedObject pSettingSO)
    {
        pSettingSO.ApplyModifiedProperties();
        EditorUtility.SetDirty(pSettingSO.targetObject);
        AssetDatabase.SaveAssets();
    }

    #endregion Private
}
