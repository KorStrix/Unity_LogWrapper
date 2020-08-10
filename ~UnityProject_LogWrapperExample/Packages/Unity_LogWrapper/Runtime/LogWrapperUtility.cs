using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class LogWrapperUtility
{
    public static void Save_ToEditorPrefs(string strKey, object pSerializeObject)
    {
#if UNITY_EDITOR
        string strJsonText = JsonUtility.ToJson(pSerializeObject);
        EditorPrefs.SetString(strKey, strJsonText);
#endif
    }

    public static bool Load_FromEditorPrefs<T>(string strKey, ref T pLoadObject_NotNull, System.Action<string> OnError = null)
    {
#if UNITY_EDITOR
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
#endif

        return true;
    }

    public static T CreateAsset<T>() where T : ScriptableObject
    {
        T pAsset = ScriptableObject.CreateInstance<T>();

#if UNITY_EDITOR
        const string strCreateAssetPath = "Resources";

        string strAbsoluteDirectory = Application.dataPath + $"/{strCreateAssetPath}";
        if (Directory.Exists(strAbsoluteDirectory) == false)
            Directory.CreateDirectory(strAbsoluteDirectory);

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath($"Assets/{strCreateAssetPath}/New {typeof(T)}.asset");

        AssetDatabase.CreateAsset(pAsset, assetPathAndName);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();

        pAsset = (T)AssetDatabase.LoadAssetAtPath(assetPathAndName, typeof(T));
        Selection.activeObject = pAsset;
#endif

        return pAsset;
    }
}
