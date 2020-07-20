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

    const string const_strPlayerPefs_SaveKey = nameof(CustomLogType_EnableArray);

    /* enum & struct declaration                */


    /* public - Field declaration               */

    public DebugWrapperEditorSetting pEditorSetting;
    public CustomLogType_EnableArray pLogTypeEnableArray;

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
            pLogTypeEnableArray = new CustomLogType_EnableArray();
            // pLogTypeEnableArray = CreateInstance<CustomLogType_EnableArray>();

        bool bIsRequireUpdate_LogTypeEnableArray = CustomLogType.Load_FromPlayerPrefs(const_strPlayerPefs_SaveKey, ref pLogTypeEnableArray) == false;
        if (bIsRequireUpdate_LogTypeEnableArray == false)
        {
            string[] arrLogTypeName_EditorSetting = pEditorSetting.arrDebugFilter.Select(p => p.strLogTypeName).ToArray();
            string[] arrLogTypeName_PlayerPrefs = pLogTypeEnableArray.arrLogEnable.Select(p => p.strCustomLogName).ToArray();

            var arrIntersect = arrLogTypeName_EditorSetting.Intersect(arrLogTypeName_PlayerPrefs);
            bIsRequireUpdate_LogTypeEnableArray = arrIntersect.Count() != arrLogTypeName_EditorSetting.Length;
        }

        if (bIsRequireUpdate_LogTypeEnableArray)
        {
            List<CustomLogType_Enable> listLogTypeEnable = new List<CustomLogType_Enable>();
            foreach (var pLogType in pEditorSetting.arrDebugFilter)
            {
                var pLogTypeEnable = pLogTypeEnableArray.arrLogEnable.FirstOrDefault(p => p.strCustomLogName == pLogType.strLogTypeName);
                if (pLogTypeEnable == null)
                    pLogTypeEnable = new CustomLogType_Enable(pLogType.strLogTypeName);

                listLogTypeEnable.Add(pLogTypeEnable);
            }

            pLogTypeEnableArray.arrLogEnable = listLogTypeEnable.ToArray();
            CustomLogType.Save_ToPlayerPrefs(const_strPlayerPefs_SaveKey, pLogTypeEnableArray);
        }
    }

    private void Draw_EditorSetting(SerializedObject pSO)
    {
        SerializedProperty pProperty = pSO.FindProperty($"{nameof(pEditorSetting)}");
        EditorGUILayout.PropertyField(pProperty);
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
            if (GUILayout.Button("Create Setting File"))
            {
                CreateAsset<DebugWrapperEditorSetting>();
                Debug.Log("Create");
            }
        }

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
            foreach (var pFilter in pEditorSetting.arrDebugFilter)
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
