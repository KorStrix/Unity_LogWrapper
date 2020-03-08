using UnityEngine;
using System.Collections.Generic;

using Object = UnityEngine.Object;
using System.Linq;

#pragma warning disable CS0419 // cref 특성에 모호한 참조가 있음
#pragma warning disable CS1573 // 매개 변수와 짝이 맞는 매개 변수 태그가 XML 주석에 없습니다. 다른 매개 변수는 짝이 맞는 태그가 있습니다.

namespace Wrapper
{
    public static partial class Debug
    {
#pragma warning disable CS1591 // 공개된 형식 또는 멤버에 대한 XML 주석이 없습니다.
        static public void Log(object message, Object context)
        {
            Log_Custom(strDefaultFlagName, message, context);
        }

        public static void Log(object message)
        {
            Log_Custom(strDefaultFlagName, message, null);
        }

        public static void LogError(object message, Object context)
        {
            LogError_Custom(strDefaultFlagName, message, context);
        }

        public static void LogError(object message)
        {
            LogError_Custom(strDefaultFlagName, message, null);
        }

        public static void LogErrorFormat(string format, params object[] args)
        {
            LogError_Custom(strDefaultFlagName, string.Format(strDefaultFlagName, args), null);
        }

        public static void LogErrorFormat(Object context, string format, params object[] args)
        {
            LogError_Custom(strDefaultFlagName, string.Format(strDefaultFlagName, args), context);
        }

        public static void LogFormat(Object context, string format, params object[] args)
        {
            Log_Custom(strDefaultFlagName, string.Format(strDefaultFlagName, args), context);
        }

        public static void LogFormat(string format, params object[] args)
        {
            Log_Custom(strDefaultFlagName, string.Format(strDefaultFlagName, args), null);
        }

        public static void LogWarning(object message)
        {
            LogWarning_Custom(strDefaultFlagName, message, null);
        }

        public static void LogWarning(object message, Object context)
        {
            LogWarning_Custom(strDefaultFlagName, message, context);
        }

        public static void LogWarningFormat(string format, params object[] args)
        {
            LogWarning_Custom(strDefaultFlagName, string.Format(strDefaultFlagName, args), null);
        }

        public static void LogWarningFormat(Object context, string format, params object[] args)
        {
            LogWarning_Custom(strDefaultFlagName, string.Format(strDefaultFlagName, args), context);
        }

#pragma warning restore CS1591 // 공개된 형식 또는 멤버에 대한 XML 주석이 없습니다.
    }
}
