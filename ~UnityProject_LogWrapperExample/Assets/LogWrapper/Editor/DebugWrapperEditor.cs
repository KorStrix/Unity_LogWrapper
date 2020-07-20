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

using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using Wrapper;
using Debug = UnityEngine.Debug;

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
        public int iValue;
    }

    /* public - Field declaration               */

    public DebugWrapperSetting pDebugInfo;
    public DebugWrapperEditorSetting pEditorSetting;

    /* protected & private - Field declaration  */

    string _strCSExportPath;

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
        EditorGUILayout.Separator();

        SetDebugInfo();
        if (GUI.changed)
        {
            Wrapper.LogFilterFlag.Save_ToPlayerPrefs(strSaveKey, pDebugInfo);
        }

        SerializedObject pSO = new SerializedObject(this);
        Draw_EditorSetting(pSO);
        Draw_CSExportButton();

        if (GUI.changed)
        {
            pSO.ApplyModifiedProperties();
            EditorUtility.SetDirty(this);
        }
    }

    private void Draw_CSExportButton()
    {
        const string strExportCS = "Export CS";
        _strCSExportPath = EditorGUILayout.TextField($"{strExportCS} Path (Assets/*.cs)", _strCSExportPath);
        if (GUILayout.Button(strExportCS))
        {
            if (string.IsNullOrEmpty(_strCSExportPath))
            {
                Debug.LogError($"{strExportCS} - string.IsNullOrEmpty(_strCSExportPath)");
                return;
            }

            CustomCodedom pCodeDom = new CustomCodedom();
            foreach (var pFilter in pEditorSetting.arrDebugFilter)
                pCodeDom.DoAddClass(pFilter);
            pCodeDom.DoExportCS($"{Application.dataPath}/{_strCSExportPath}");

            AssetDatabase.Refresh();
            Debug.Log($"{strExportCS} Complete");
        }
    }

    /* protected - [abstract & virtual]         */


    // ========================================================================== //

    #region Private

    private void SetDebugInfo()
    {
        if (pDebugInfo != null)
            return;

        pDebugInfo = new DebugWrapperSetting();
        if (LogFilterFlag.Load_FromPlayerPrefs(strSaveKey, ref pDebugInfo) == false)
            LogFilterFlag.Save_ToPlayerPrefs(strSaveKey, pDebugInfo);
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

    #endregion Private

    #region Tool


    public static T CreateAsset<T>() where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();

        string path = "Assets";
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(T) + ".asset");

        AssetDatabase.CreateAsset(asset, assetPathAndName);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;

        return asset;
    }


    #endregion Tool
}
