using CustomDebug;
using UnityEngine;
using System.Text;

#if UNITY_EDITOR
using UnityEditor;
#endif

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


    public string Comment { get; }
    public string LogTypeName => strLogTypeName;
    public ulong Number => lNumber;
    public string ColorHexCode => strColorHexCode;

    /// <summary>
    /// 디버그 필터 플래그
    /// </summary>
    public string strLogTypeName;

    /// <summary>
    /// 주석
    /// </summary>
    public string strComment;

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
        StringBuilder strBuilder = new StringBuilder();
        strBuilder.AppendLine("    /// <summary>");
        strBuilder.AppendLine($"    /// {strComment}");
        strBuilder.AppendLine("    /// </summary>");
        strBuilder.AppendLine($"    public static {nameof(CustomLogType)} {strLogTypeName} = new CustomLogType(\"{strLogTypeName}\", {lNumber}, \"{strColorHexCode}\");");

        return strBuilder.ToString();
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

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(CustomLogType))]
public class CustomLogTypeDrawer : PropertyDrawer
{
    private const float fLabelWidth_strLogTypeName = 100f;
    private const float fLabelWidth_lNumber = 50f;
    private const float fLabelWidth_strColorHexCode = 50f;


    private const float fLabelOffset = 5f;


    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        {
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            position.height = 17f;
            float fOriginX = position.x;

            SerializedProperty pProperty_strLogTypeName = property.FindPropertyRelative(nameof(CustomLogType.strLogTypeName));
            EditorGUI.PropertyField(CalculateRect(ref position, fLabelWidth_strLogTypeName, fLabelOffset), pProperty_strLogTypeName, GUIContent.none);

            SerializedProperty pProperty_lNumber = property.FindPropertyRelative(nameof(CustomLogType.lNumber));
            EditorGUI.PropertyField(CalculateRect(ref position, fLabelWidth_lNumber, fLabelOffset), pProperty_lNumber, GUIContent.none);

            // Color
            SerializedProperty pProperty_strColorHexCode = property.FindPropertyRelative(nameof(CustomLogType.strColorHexCode));

            // Editor는 #RGBA를 원하고 Code는 RGB만 필요
            ColorUtility.TryParseHtmlString("#" + pProperty_strColorHexCode.stringValue + "FF", out Color sColor);
            sColor = EditorGUI.ColorField(CalculateRect(ref position, fLabelWidth_strColorHexCode, fLabelOffset), sColor);

            pProperty_strColorHexCode.stringValue = ColorUtility.ToHtmlStringRGB(sColor);


            position.x = fOriginX;
            position.y += 20f;

            SerializedProperty pProperty_strComment = property.FindPropertyRelative(nameof(CustomLogType.strComment));
            EditorGUI.PropertyField(position, pProperty_strComment, new GUIContent("Comment"));

            label.text = $"{pProperty_strLogTypeName.stringValue}[{pProperty_lNumber.longValue}]";
        }
        EditorGUI.EndProperty();
    }

    private static Rect CalculateRect(ref Rect position, float fLabelWidth, float fOffset)
    {
        var rectFlagName = new Rect(position.x, position.y, fLabelWidth, position.height);

        position.x += fLabelWidth + fOffset;

        return rectFlagName;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) + 30f;
    }
}
#endif
