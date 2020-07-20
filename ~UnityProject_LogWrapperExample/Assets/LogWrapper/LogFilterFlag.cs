using System.Collections.Generic;
using UnityEngine;
using Wrapper;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Wrapper
{
    /// <summary>
    /// 디버그 필터
    /// </summary>
    [System.Serializable]
    public partial class LogFilterFlag
    {
        #region DefaultFilter
        /// <summary>
        /// <see cref="Debug.Log(object)"/>로 출력하고 싶은 경우 이 플래그를 넣으시면 됩니다
        /// </summary>
        public static LogFilterFlag Log = new LogFilterFlag(nameof(Log), 1 << 0);

        /// <summary>
        /// <see cref="Debug.LogWarning(object)"/>로 출력하고 싶은 경우 이 플래그를 넣으시면 됩니다
        /// </summary>
        public static LogFilterFlag Warning = new LogFilterFlag(nameof(Warning), 1 << 1, "ffff00");

        /// <summary>
        /// <see cref="Debug.LogError(object)"/>로 출력하고 싶은 경우 이 플래그를 넣으시면 됩니다.
        /// </summary>
        public static LogFilterFlag Error = new LogFilterFlag(nameof(Error), 1 << 2, "ff0000");
        #endregion

        /// <summary>
        /// 디버그 필터 플래그
        /// </summary>
        public string strFlagName;

        /// <summary>
        /// 플래그 체크용 ulong 값
        /// </summary>
        public ulong lNumber;

        /// <summary>
        /// 필터의 정보
        /// <para>Ex) 흰색 : ffffff</para>
        /// <para>Ex) 빨간색 : ff0000</para>
        /// </summary>
        public string strColorHexCode = "ffffff";

        /// <summary>
        /// 필터의 정보
        /// </summary>
        /// <param name="strFlagName">디버그 필터 플래그</param>
        public LogFilterFlag(string strFlagName)
        {
            this.strFlagName = strFlagName;
        }

        /// <summary>
        /// 필터의 정보
        /// </summary>
        /// <param name="strFlagName">디버그 필터 플래그</param>
        /// <param name="lNumber">플래그 체크할 숫자</param>
        public LogFilterFlag(string strFlagName, ulong lNumber)
        {
            this.strFlagName = strFlagName;
            this.lNumber = lNumber;
        }

        /// <summary>
        /// 필터의 정보
        /// </summary>
        /// <param name="strFlagName">디버그 필터 플래그</param>
        /// <param name="lNumber">플래그 체크할 숫자</param>
        /// <param name="strColorHexCode">색상 코드 (Ex. 흰색 : ffffff)</param>
        public LogFilterFlag(string strFlagName, ulong lNumber, string strColorHexCode)
        {
            this.strFlagName = strFlagName;
            this.lNumber = lNumber;
            this.strColorHexCode = strColorHexCode;
        }

        public string ToCSharpCodeString()
        {
            return $@"  public static {nameof(LogFilterFlag)} {strFlagName} = new LogFilterFlag(""{strFlagName}"", {lNumber}, ""{strColorHexCode}"");";
        }



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
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(LogFilterFlag))]
public class DebugFilterDrawer : PropertyDrawer
{
    private const float fLabelWidth_FlagName = 80f;
    private const float fLabelWidth_lNumber = 50f;
    private const float fLabelWidth_strColorHexCode = 50f;


    private const float fLabelOffset = 5f;


    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        {
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);



            SerializedProperty pProperty_strFlagName = property.FindPropertyRelative(nameof(LogFilterFlag.strFlagName));
            EditorGUI.PropertyField(CalculateRect(ref position, fLabelWidth_FlagName, fLabelOffset), pProperty_strFlagName, GUIContent.none);

            SerializedProperty pProperty_lNumber = property.FindPropertyRelative(nameof(LogFilterFlag.lNumber));
            EditorGUI.PropertyField(CalculateRect(ref position, fLabelWidth_lNumber, fLabelOffset), pProperty_lNumber, GUIContent.none);

            // Color
            SerializedProperty pProperty_strColorHexCode = property.FindPropertyRelative(nameof(LogFilterFlag.strColorHexCode));
            ColorUtility.TryParseHtmlString(pProperty_strColorHexCode.stringValue, out Color sColor);
            sColor = EditorGUI.ColorField(CalculateRect(ref position, fLabelWidth_strColorHexCode, fLabelOffset), sColor);
            pProperty_strColorHexCode.stringValue = ColorUtility.ToHtmlStringRGB(sColor);


            label.text = $"{pProperty_strFlagName.stringValue}[{pProperty_lNumber.longValue}]";
        }
        EditorGUI.EndProperty();
    }

    private static Rect CalculateRect(ref Rect position, float fLabelWidth, float fOffset)
    {
        var rectFlagName = new Rect(position.x, position.y, fLabelWidth, position.height);

        position.x += fLabelWidth + fOffset;

        return rectFlagName;
    }
}
#endif
