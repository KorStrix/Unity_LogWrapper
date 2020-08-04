using UnityEngine;

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
}
