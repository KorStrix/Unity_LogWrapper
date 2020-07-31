using CustomDebug;

//namespace Wrapper
//{
/// <summary>
/// 디버그 필터
/// </summary>
[System.Serializable]
public class CustomLogType : ICustomLogType
{
    #region DefaultFilter
    /// <summary>
    /// <see cref="Debug.Log(object)"/>로 출력하고 싶은 경우 이 플래그를 넣으시면 됩니다
    /// </summary>
    public static CustomLogType Log = new CustomLogType(nameof(Log), 1 << 0);

    /// <summary>
    /// <see cref="Debug.LogWarning(object)"/>로 출력하고 싶은 경우 이 플래그를 넣으시면 됩니다
    /// </summary>
    public static CustomLogType Warning = new CustomLogType(nameof(Warning), 1 << 1, "ffff00");

    /// <summary>
    /// <see cref="Debug.LogError(object)"/>로 출력하고 싶은 경우 이 플래그를 넣으시면 됩니다.
    /// </summary>
    public static CustomLogType Error = new CustomLogType(nameof(Error), 1 << 2, "ff0000");
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

    public CustomLogType()
    {
    }

    /// <summary>
    /// 필터의 정보
    /// </summary>
    /// <param name="strLogTypeName">디버그 필터 플래그</param>
    /// <param name="lNumber">플래그 체크할 숫자</param>
    /// <param name="strColorHexCode">색상 코드 (Ex. 흰색 : ffffff)</param>
    public CustomLogType(string strLogTypeName, ulong lNumber, string strColorHexCode = "ffffff")
    {
        this.strLogTypeName = strLogTypeName;
        this.lNumber = lNumber;
        this.strColorHexCode = strColorHexCode;
    }

    public string ToCSharpCodeString()
    {
        return $@"public static {nameof(CustomLogType)} {strLogTypeName} = new CustomLogType(""{strLogTypeName}"", {lNumber}, ""{strColorHexCode}"");";
    }

    #region operator

    public static CustomLogType operator |(CustomLogType a, CustomLogType b)
    {
        CustomLogType pNewLogType = new CustomLogType(
            $"({a.strLogTypeName}|{b.strLogTypeName})", a.lNumber | b.lNumber);

        return pNewLogType;
    }

    public static CustomLogType operator &(CustomLogType a, CustomLogType b)
    {
        CustomLogType pNewLogType = new CustomLogType(
            $"({a.strLogTypeName}&{b.strLogTypeName})", a.lNumber & b.lNumber);

        return pNewLogType;
    }

    #endregion


}
//}
