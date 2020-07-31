﻿using Object = UnityEngine.Object;

#pragma warning disable CS0419 // cref 특성에 모호한 참조가 있음
#pragma warning disable CS1573 // 매개 변수와 짝이 맞는 매개 변수 태그가 XML 주석에 없습니다. 다른 매개 변수는 짝이 맞는 태그가 있습니다.

//namespace Wrapper
//{
    public static partial class Debug
    {
#pragma warning disable CS1591 // 공개된 형식 또는 멤버에 대한 XML 주석이 없습니다.
        public static void Log(object message, Object context)
        {
            Log_Custom(Default, message, context);
        }

        public static void Log(object message)
        {
            Log_Custom(Default, message, null);
        }

        public static void LogError(object message, Object context)
        {
            LogError_Custom(Default, message, context);
        }

        public static void LogError(object message)
        {
            LogError_Custom(Default, message, null);
        }

        public static void LogErrorFormat(string format, params object[] args)
        {
            LogError_Custom(Default, string.Format(format, args), null);
        }

        public static void LogErrorFormat(Object context, string format, params object[] args)
        {
            LogError_Custom(Default, string.Format(format, args), context);
        }

        public static void LogFormat(Object context, string format, params object[] args)
        {
            Log_Custom(Default, string.Format(format, args), context);
        }

        public static void LogFormat(string format, params object[] args)
        {
            Log_Custom(Default, string.Format(format, args), null);
        }

        public static void LogWarning(object message)
        {
            LogWarning_Custom(Default, message, null);
        }

        public static void LogWarning(object message, Object context)
        {
            LogWarning_Custom(Default, message, context);
        }

        public static void LogWarningFormat(string format, params object[] args)
        {
            LogWarning_Custom(Default, string.Format(format, args), null);
        }

        public static void LogWarningFormat(Object context, string format, params object[] args)
        {
            LogWarning_Custom(Default, string.Format(format, args), context);
        }

#pragma warning restore CS1591 // 공개된 형식 또는 멤버에 대한 XML 주석이 없습니다.
    }
//}
