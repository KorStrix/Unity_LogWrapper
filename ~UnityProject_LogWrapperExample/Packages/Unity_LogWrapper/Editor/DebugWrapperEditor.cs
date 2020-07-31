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

    private void Get_LogTypeEnable_FromPlayerPrefs(SerializedObject pSO)
    {
        bool bIsSave = false;

        if (pLocalBranch == null)
            pLocalBranch = LogFilter_PerBranch.Get_LogTypeEnable_FromPlayerPrefs(out bIsSave);

        if (bIsSave)
        {
            CustomLogType_Enable.DoMatch_LogTypeEnableArray(pEditorSetting, ref pLocalBranch.arrLogTypeEnable);
            LogWrapperUtility.Save_ToPlayerPrefs(LogFilter_PerBranch.const_strPlayerPefs_SaveKey, pLocalBranch);
        }

        if (pLocalBranch.pEditorSetting != pEditorSetting)
        {
            pLocalBranch.pEditorSetting = pEditorSetting;
            bIsSave = true;
        }

        if (bIsSave)
        {
            pSO.ApplyModifiedProperties();
            EditorUtility.SetDirty(this);
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
        Get_LogTypeEnable_FromPlayerPrefs(pSO);

        SerializedProperty pProperty = pSO.FindProperty($"{nameof(pLocalBranch)}");
        EditorGUILayout.PropertyField(pProperty, true);

        if (GUI.changed)
        {
            pSO.ApplyModifiedProperties();
            EditorUtility.SetDirty(this);
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
                if (string.IsNullOrEmpty(pEditorSetting.strCSExportPath))
                {
                    Debug.LogError($"{strExportCS} - string.IsNullOrEmpty(_strCSExportPath)");
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
        Debug.Log($"{strExportCS} Complete");
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
