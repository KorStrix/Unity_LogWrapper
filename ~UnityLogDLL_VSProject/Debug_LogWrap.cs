using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
using CustomDebug;
using System.Runtime.CompilerServices;

using Object = UnityEngine.Object;

namespace Wrapper
{

    public static partial class Debug
    {
        /// <summary>
        /// <see cref="UnityEngine.Debug.Log(object)"/>를 출력합니다.
        /// <para><see cref="DoInit_PrintLog_FilterFlag"/>에서 세팅한 플래그가 아니면 출력하지 않습니다.</para>
        /// </summary>
        /// <param name="pFilterFlags">출력할 필터 플래그입니다</param>
        /// <param name="message">로그 메시지</param>
        public static void Log(ICustomLogType pFilterFlags, object message, [CallerMemberName] string strMemberName = "", [CallerFilePath] string strFilePath = "", [CallerLineNumber] int iSourceLineNumber = -1)
        {
            Log_Custom(pFilterFlags, Environment.StackTrace, message, null, strMemberName, strFilePath, iSourceLineNumber);
        }

        /// <summary>
        /// <see cref="UnityEngine.Debug.Log(object, Object)"/>를 출력합니다.
        /// <para><see cref="DoInit_PrintLog_FilterFlag"/>에서 세팅한 플래그가 아니면 출력하지 않습니다.</para>
        /// </summary>
        /// <param name="pFilterFlags">출력할 필터 플래그입니다</param>
        /// <param name="message">로그 메시지</param>
        /// <param name="context">유니티 오브젝트</param>
        public static void Log(ICustomLogType pFilterFlags, object message, Object context, [CallerMemberName] string strMemberName = "", [CallerFilePath] string strFilePath = "", [CallerLineNumber] int iSourceLineNumber = -1)
        {
            Log_Custom(pFilterFlags, Environment.StackTrace, message, context, strMemberName, strFilePath, iSourceLineNumber);
        }

        /// <summary>
        /// <see cref="UnityEngine.Debug.LogError(object)"/>를 출력합니다.
        /// <para><see cref="DoInit_PrintLog_FilterFlag"/>에서 세팅한 플래그가 아니면 출력하지 않습니다.</para>
        /// </summary>
        /// <param name="pFilterFlags">출력할 필터 플래그입니다</param>
        /// <param name="message">로그 에러 메시지</param>
        public static void LogError(ICustomLogType pFilterFlags, object message, [CallerMemberName] string strMemberName = "", [CallerFilePath] string strFilePath = "", [CallerLineNumber] int iSourceLineNumber = -1)
        {
            LogError_Custom(pFilterFlags, Environment.StackTrace, message, null, strMemberName, strFilePath, iSourceLineNumber);
        }

        /// <summary>
        /// <see cref="UnityEngine.Debug.LogError(object, Object)"/>를 출력합니다.
        /// <para><see cref="DoInit_PrintLog_FilterFlag"/>에서 세팅한 플래그가 아니면 출력하지 않습니다.</para>
        /// </summary>
        /// <param name="pFilterFlags">출력할 필터 플래그입니다</param>
        /// <param name="message">로그 에러 메시지</param>
        public static void LogError(ICustomLogType pFilterFlags, object message, Object context, [CallerMemberName] string strMemberName = "", [CallerFilePath] string strFilePath = "", [CallerLineNumber] int iSourceLineNumber = -1)
        {
            LogError_Custom(pFilterFlags, Environment.StackTrace, message, context, strMemberName, strFilePath, iSourceLineNumber);
        }

        /// <summary>
        /// <see cref="UnityEngine.Debug.LogWarning(object)"/>를 출력합니다.
        /// <para><see cref="DoInit_PrintLog_FilterFlag"/>에서 세팅한 플래그가 아니면 출력하지 않습니다.</para>
        /// </summary>
        /// <param name="pFilterFlags">출력할 필터 플래그입니다</param>
        /// <param name="message">로그 에러 메시지</param>
        public static void LogWarning(ICustomLogType pFilterFlags, object message, [CallerMemberName] string strMemberName = "", [CallerFilePath] string strFilePath = "", [CallerLineNumber]int iSourceLineNumber = -1)
        {
            LogWarning_Custom(pFilterFlags, Environment.StackTrace, message, null, strMemberName, strFilePath, iSourceLineNumber);
        }

        /// <summary>
        /// <see cref="UnityEngine.Debug.LogWarning(object, Object)"/>를 출력합니다.
        /// <para><see cref="DoInit_PrintLog_FilterFlag"/>에서 세팅한 플래그가 아니면 출력하지 않습니다.</para>
        /// </summary>
        /// <param name="pFilterFlags">출력할 필터 플래그입니다</param>
        /// <param name="message">로그 에러 메시지</param>
        public static void LogWarning(ICustomLogType pFilterFlags, object message, Object context, [CallerMemberName] string strMemberName = "", [CallerFilePath] string strFilePath = "", [CallerLineNumber] int iSourceLineNumber = -1)
        {
            LogWarning_Custom(pFilterFlags, Environment.StackTrace, message, context, strMemberName, strFilePath, iSourceLineNumber);
        }

        #region LogWrapping

        private static void Log_Custom(ICustomLogType pFilterFlags, string strStacktrace, object message, Object context, string strMemberName = "", string strFilePath = "", int iSourceLineNumber = -1)
        {
            if (Check_IsContainFilter(_ulFilterFlags, pFilterFlags) == false)
                return;

            PrintLog(pFilterFlags, strStacktrace, message, context, LogType.Log, strMemberName, strFilePath, iSourceLineNumber);
        }

        private static void LogWarning_Custom(ICustomLogType pFilterFlags, string strStacktrace, object message, Object context, string strMemberName = "", string strFilePath = "", int iSourceLineNumber = -1)
        {
            if (Check_IsContainFilter(_ulFilterFlags, pFilterFlags) == false)
                return;

            PrintLog(pFilterFlags, strStacktrace, message, context, LogType.Warning, strMemberName, strFilePath, iSourceLineNumber);
        }

        private static void LogError_Custom(ICustomLogType pFilterFlags, string strStacktrace, object message, Object context, string strMemberName = "", string strFilePath = "", int iSourceLineNumber = -1)
        {
            PrintLog(pFilterFlags, strStacktrace, message, context, LogType.Error, strMemberName, strFilePath, iSourceLineNumber);
        }

        private static void LogException_Custom(Exception message, Object context)
        {
            UnityEngine.Debug.LogException(message, context);
        }

        #endregion
    }
}
