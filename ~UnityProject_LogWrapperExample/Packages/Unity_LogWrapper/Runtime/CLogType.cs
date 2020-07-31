﻿using CustomDebug;
using UnityEngine;

//namespace Wrapper
//{
/// <summary>
/// 디버그 필터
/// </summary>
[System.Serializable]
public partial class CLogType : ICustomLogType
{
    #region DefaultFilter
    /// <summary>
    /// <see cref="Debug.Log(object)"/>로 출력하고 싶은 경우 이 플래그를 넣으시면 됩니다
    /// </summary>
    public static CLogType Log = new CLogType(nameof(Log), 1 << 0);

    /// <summary>
    /// <see cref="Debug.LogWarning(object)"/>로 출력하고 싶은 경우 이 플래그를 넣으시면 됩니다
    /// </summary>
    public static CLogType Warning = new CLogType(nameof(Warning), 1 << 1, "ffff00");

    /// <summary>
    /// <see cref="Debug.LogError(object)"/>로 출력하고 싶은 경우 이 플래그를 넣으시면 됩니다.
    /// </summary>
    public static CLogType Error = new CLogType(nameof(Error), 1 << 2, "ff0000");
    #endregion


    public string LogTypeName => strLogTypeName;
    public ulong Number => lNumber;
    public string ColorHexCode => strColorHexCode;


    /// <summary>
    /// 디버그 필터 플래그
    /// </summary>
    public string strLogTypeName;

    /// <summary>
    /// 플래그 체크용 ulong 값
    /// </summary>
    public ulong lNumber;

    /// <summary>
    /// 필터의 정보
    /// <para>Ex) 흰색 : ffffff</para>
    /// <para>Ex) 빨간색 : ff0000</para>
    /// </summary>
    public string strColorHexCode;

    /// <summary>
    /// 필터의 정보
    /// </summary>
    /// <param name="strLogTypeName">디버그 필터 플래그</param>
    public CLogType(string strLogTypeName)
    {
        this.strLogTypeName = strLogTypeName;
        this.lNumber = 0;
        this.strColorHexCode = "ffffff";
    }

    /// <summary>
    /// 필터의 정보
    /// </summary>
    /// <param name="strLogTypeName">디버그 필터 플래그</param>
    /// <param name="lNumber">플래그 체크할 숫자</param>
    public CLogType(string strLogTypeName, ulong lNumber)
    {
        this.strLogTypeName = strLogTypeName;
        this.lNumber = lNumber;
        this.strColorHexCode = "ffffff";
    }

    /// <summary>
    /// 필터의 정보
    /// </summary>
    /// <param name="strLogTypeName">디버그 필터 플래그</param>
    /// <param name="lNumber">플래그 체크할 숫자</param>
    /// <param name="strColorHexCode">색상 코드 (Ex. 흰색 : ffffff)</param>
    public CLogType(string strLogTypeName, ulong lNumber, string strColorHexCode)
    {
        this.strLogTypeName = strLogTypeName;
        this.lNumber = lNumber;
        this.strColorHexCode = strColorHexCode;
    }

    public string ToCSharpCodeString()
    {
        return $@"  public static {nameof(CLogType)} {strLogTypeName} = new CLogType(""{strLogTypeName}"", {lNumber}, ""{strColorHexCode}"");";
    }

    #region operator

    public static CLogType operator |(CLogType a, CLogType b)
    {
        CLogType pNewLogType = new CLogType(
            $"({a.strLogTypeName}|{b.strLogTypeName})", a.lNumber | b.lNumber);

        return pNewLogType;
    }

    public static CLogType operator &(CLogType a, CLogType b)
    {
        CLogType pNewLogType = new CLogType(
            $"({a.strLogTypeName}&{b.strLogTypeName})", a.lNumber & b.lNumber);

        return pNewLogType;
    }

    #endregion


    #region Tool
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
    #endregion Tool
}
//}
