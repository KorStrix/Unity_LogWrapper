using UnityEngine;
using Wrapper;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Wrapper
{
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(CLogType))]
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



                SerializedProperty pProperty_strLogTypeName = property.FindPropertyRelative(nameof(CLogType.strLogTypeName));
                EditorGUI.PropertyField(CalculateRect(ref position, fLabelWidth_strLogTypeName, fLabelOffset), pProperty_strLogTypeName, GUIContent.none);

                SerializedProperty pProperty_lNumber = property.FindPropertyRelative(nameof(CLogType.lNumber));
                EditorGUI.PropertyField(CalculateRect(ref position, fLabelWidth_lNumber, fLabelOffset), pProperty_lNumber, GUIContent.none);

                // Color
                SerializedProperty pProperty_strColorHexCode = property.FindPropertyRelative(nameof(CLogType.strColorHexCode));

                // Editor는 #RGBA를 원하고 Code는 RGB만 필요
                ColorUtility.TryParseHtmlString("#" + pProperty_strColorHexCode.stringValue + "FF", out Color sColor);
                sColor = EditorGUI.ColorField(CalculateRect(ref position, fLabelWidth_strColorHexCode, fLabelOffset), sColor);

                pProperty_strColorHexCode.stringValue = ColorUtility.ToHtmlStringRGB(sColor);

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
    }
}
#endif
