using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Object = UnityEngine.Object;

#pragma warning disable CS0419 // cref 특성에 모호한 참조가 있음
#pragma warning disable CS1573 // 매개 변수와 짝이 맞는 매개 변수 태그가 XML 주석에 없습니다. 다른 매개 변수는 짝이 맞는 태그가 있습니다.

namespace Wrapper {

    public static partial class Debug
    {
#pragma warning disable CS1591 // 공개된 형식 또는 멤버에 대한 XML 주석이 없습니다.
        public static void Log(object message, Object context)
        {
            Log_Custom(Default, Environment.StackTrace, message, context);
        }

        public static void Log(object message)
        {
            Log_Custom(Default, Environment.StackTrace, message, null);
        }

        public static void LogFormat(Object context, string format, params object[] args)
        {
            Log_Custom(Default, Environment.StackTrace, string.Format(format, args), context, "", "", -1);
        }

        public static void LogFormat(string format, params object[] args)
        {
            Log_Custom(Default, Environment.StackTrace, string.Format(format, args), null, "", "", -1);
        }

        public static void LogWarning(object message)
        {
            LogWarning_Custom(Default, Environment.StackTrace, message, null);
        }

        public static void LogWarning(object message, Object context)
        {
            LogWarning_Custom(Default, Environment.StackTrace, message, context);
        }

        public static void LogWarningFormat(string format, params object[] args)
        {
            LogWarning_Custom(Default, Environment.StackTrace, string.Format(format, args), null, "", "", -1);
        }

        public static void LogWarningFormat(Object context, string format, params object[] args)
        {
            LogWarning_Custom(Default, Environment.StackTrace, string.Format(format, args), context, "", "", -1);
        }

        public static void LogError(object message, Object context)
        {
            LogError_Custom(Default, Environment.StackTrace, message, context);
        }

        public static void LogException(System.Exception exception)
        {
            LogException_Custom(exception, null);
        }
        public static void LogException(System.Exception exception, Object context)
        {
            LogException_Custom(exception, context);
        }

        public static void LogError(object message)
        {
            LogError_Custom(Default, Environment.StackTrace, message, null);
        }

        public static void LogErrorFormat(string format, params object[] args)
        {
            LogError_Custom(Default, Environment.StackTrace, string.Format(format, args), null, "", "", -1);
        }

        public static void LogErrorFormat(Object context, string format, params object[] args)
        {
            LogError_Custom(Default, Environment.StackTrace, string.Format(format, args), context, "", "", -1);
        }

        [Conditional("UNITY_ASSERTIONS")]
        public static void Assert(bool condition)
        {
            if (condition)
                return;
            UnityEngine.Debug.LogAssertion((object)"Assertion failed");
        }

        [Conditional("UNITY_ASSERTIONS")]
        public static void Assert(bool condition, Object context)
        {
            if (condition)
                return;
            UnityEngine.Debug.LogAssertion((object)"Assertion failed", context);
        }

        [Conditional("UNITY_ASSERTIONS")]
        public static void Assert(bool condition, object message)
        {
            if (condition)
                return;
            UnityEngine.Debug.LogAssertion(message);
        }

        [Conditional("UNITY_ASSERTIONS")]
        public static void Assert(bool condition, string message)
        {
            if (condition)
                return;
            UnityEngine.Debug.LogAssertion((object)message);
        }

        [Conditional("UNITY_ASSERTIONS")]
        public static void Assert(bool condition, object message, Object context)
        {
            if (condition)
                return;
            UnityEngine.Debug.LogAssertion(message, context);
        }

        [Conditional("UNITY_ASSERTIONS")]
        public static void Assert(bool condition, string message, Object context)
        {
            if (condition)
                return;
            UnityEngine.Debug.LogAssertion((object)message, context);
        }

#pragma warning restore CS1591 // 공개된 형식 또는 멤버에 대한 XML 주석이 없습니다.
    }
}
