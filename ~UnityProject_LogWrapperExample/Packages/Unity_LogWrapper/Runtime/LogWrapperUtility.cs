using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class LogWrapperUtility
{
    public static void Save_ToPlayerPrefs(string strKey, object pSerializeObject)
    {
        string strJsonText = JsonUtility.ToJson(pSerializeObject);
        PlayerPrefs.SetString(strKey, strJsonText);
    }

    public static bool Load_FromPlayerPrefs<T>(string strKey, ref T pLoadObject_NotNull, System.Action<string> OnError = null)
    {
        if (PlayerPrefs.HasKey(strKey) == false)
        {
            OnError?.Invoke($"{nameof(Load_FromPlayerPrefs)} - PlayerPrefs.HasKey({strKey}) == false");

            return false;
        }

        string strJson = PlayerPrefs.GetString(strKey);
        try
        {
            JsonUtility.FromJsonOverwrite(strJson, pLoadObject_NotNull);
        }
        catch (System.Exception e)
        {
            OnError?.Invoke($"{nameof(Load_FromPlayerPrefs)} - FromJsonOverwrite Fail - {strKey} Value : \n{strJson}\n{e}");

            return false;
        }

        return true;
    }

    public static T CreateAsset<T>() where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();

#if UNITY_EDITOR
        const string strCreateAssetPath = "Resources";

        string strAbsoluteDirectory = Application.dataPath + $"/{strCreateAssetPath}";
        if (Directory.Exists(strAbsoluteDirectory) == false)
            Directory.CreateDirectory(strAbsoluteDirectory);

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath($"Assets/{strCreateAssetPath}/New {typeof(T)}.asset");

        AssetDatabase.CreateAsset(asset, assetPathAndName);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
#endif

        return asset;
    }
}
