using UnityEngine;
using System.Collections.Generic;

using Object = UnityEngine.Object;

namespace Wrapper
{
    /// <summary>
    /// <see cref="UnityEngine.Debug"/>를 래핑한 클래스입니다.
    /// </summary>
    public static class Debug
    {
        public struct DebugFilterInfo
        {
            public object pFilterFlag;
            public string strColorHexCode;

            public DebugFilterInfo(object pFilterFlag, string strColorHexCode)
            {
                this.pFilterFlag = pFilterFlag; this.strColorHexCode = strColorHexCode;
            }
        }

        public delegate void PrintLogFormat(object pMessage, object pFilterFlags, out string strMessageResult);

        /// <summary>
        /// 로그에 출력하는 색상
        /// <para>Ex) 008000ff</para>
        /// <para>RGBA</para>
        /// </summary>
        static public string strDefaultColorHexCode = "008000ff";

        /// <summary>
        /// 플래그를 지정하지 않은 로그에 대한 플래그
        /// </summary>
        static public string strDefaultFlagName = "Default";

        static Dictionary<int, string> _mapColorHexCode = new Dictionary<int, string>();
        static int _iFilterFlags;
        static PrintLogFormat _OnLogFormat = LogFormat_Default;

        /// <summary>
        /// 출력할 로그의 필터 <see cref="System.FlagsAttribute"/>를 지정합니다. 인자가 <see cref="System.Int32.MaxValue"/>이면 모두 출력합니다.
        /// </summary>
        static public void Set_PrintLog_FilterFlag(params object[] arrPrintFilterFlag)
        {
            _iFilterFlags = 0;
            for (int i = 0; i < arrPrintFilterFlag.Length; i++)
                _iFilterFlags |= arrPrintFilterFlag[i].GetHashCode();
        }

        /// <summary>
        /// 출력할 로그의 필터 <see cref="System.FlagsAttribute"/>를 지정합니다.
        /// </summary>
        /// <param name="arrPrintFilterFlag"></param>
        static public void Set_PrintLog_FilterFlag(params DebugFilterInfo[] arrPrintFilterFlag)
        {
            _mapColorHexCode.Clear();
            _iFilterFlags = 0;
            for (int i = 0; i < arrPrintFilterFlag.Length; i++)
            {
                if(arrPrintFilterFlag[i].pFilterFlag.Equals(strDefaultFlagName))
                {
                    strDefaultColorHexCode = arrPrintFilterFlag[i].strColorHexCode;
                }
                else
                {
                    int iHashCode = arrPrintFilterFlag[i].pFilterFlag.GetHashCode();
                    _iFilterFlags |= iHashCode;
                    _mapColorHexCode.Add(iHashCode, arrPrintFilterFlag[i].strColorHexCode);
                }
            }
        }

        /// <summary>
        /// 로그의 출력 포멧을 지정합니다.
        /// </summary>
        static public void Set_PrintLogFormat(PrintLogFormat OnLogFormat)
        {
            _OnLogFormat = OnLogFormat;
        }

        /// <summary>
        /// <see cref="UnityEngine.Debug.Log(object)"/>를 출력합니다.
        /// <para><see cref="Set_PrintLog_FilterFlag"/>에서 세팅한 플래그가 아니면 출력하지 않습니다.</para>
        /// </summary>
        /// <param name="pFilterFlags">출력할 필터 플래그입니다</param>
        /// <param name="message">로그 메시지</param>
        static public void Log(object pFilterFlags, object message)
        {
            Log_Custom(pFilterFlags, message, null);
        }

        /// <summary>
        /// <see cref="UnityEngine.Debug.Log(object, Object)"/>를 출력합니다.
        /// <para><see cref="Set_PrintLog_FilterFlag"/>에서 세팅한 플래그가 아니면 출력하지 않습니다.</para>
        /// </summary>
        /// <param name="pFilterFlags">출력할 필터 플래그입니다</param>
        /// <param name="message">로그 메시지</param>
        /// <param name="context">유니티 오브젝트</param>
        static public void Log(object pFilterFlags, object message, Object context)
        {
            Log_Custom(pFilterFlags, message, context);
        }

        /// <summary>
        /// <see cref="UnityEngine.Debug.LogError(object)"/>를 출력합니다.
        /// <para><see cref="Set_PrintLog_FilterFlag"/>에서 세팅한 플래그가 아니면 출력하지 않습니다.</para>
        /// </summary>
        /// <param name="pFilterFlags">출력할 필터 플래그입니다</param>
        /// <param name="message">로그 에러 메시지</param>
        static public void LogError(object pFilterFlags, object message)
        {
            LogError_Custom(pFilterFlags, message, null);
        }

        /// <summary>
        /// <see cref="UnityEngine.Debug.LogError(object, Object)"/>를 출력합니다.
        /// <para><see cref="Set_PrintLog_FilterFlag"/>에서 세팅한 플래그가 아니면 출력하지 않습니다.</para>
        /// </summary>
        /// <param name="pFilterFlags">출력할 필터 플래그입니다</param>
        /// <param name="message">로그 에러 메시지</param>
        static public void LogError(object pFilterFlags, object message, Object context)
        {
            LogError_Custom(pFilterFlags, message, context);
        }

        /// <summary>
        /// <see cref="UnityEngine.Debug.LogWarning(object)"/>를 출력합니다.
        /// <para><see cref="Set_PrintLog_FilterFlag"/>에서 세팅한 플래그가 아니면 출력하지 않습니다.</para>
        /// </summary>
        /// <param name="pFilterFlags">출력할 필터 플래그입니다</param>
        /// <param name="message">로그 에러 메시지</param>
        static public void LogWarning(object pFilterFlags, object message)
        {
            LogWarning_Custom(pFilterFlags, message, null);
        }

        /// <summary>
        /// <see cref="UnityEngine.Debug.LogWarning(object, Object)"/>를 출력합니다.
        /// <para><see cref="Set_PrintLog_FilterFlag"/>에서 세팅한 플래그가 아니면 출력하지 않습니다.</para>
        /// </summary>
        /// <param name="pFilterFlags">출력할 필터 플래그입니다</param>
        /// <param name="message">로그 에러 메시지</param>
        static public void LogWarning(object pFilterFlags, object message, Object context)
        {
            LogWarning_Custom(pFilterFlags, message, context);
        }

        #region LogWrapping
        static private void Log_Custom(object pFilterFlags, object message, Object context)
        {
            if (Check_IsFiltering(pFilterFlags))
                return;

            string strMessageOut;
            _OnLogFormat(message, pFilterFlags, out strMessageOut);

            UnityEngine.Debug.Log(strMessageOut, context);
        }

        static private void LogError_Custom(object pFilterFlags, object message, Object context)
        {
            if (Check_IsFiltering(pFilterFlags))
                return;

            string strMessageOut;
            _OnLogFormat(message, pFilterFlags, out strMessageOut);

            UnityEngine.Debug.LogError(strMessageOut, context);
        }

        static private void LogWarning_Custom(object pFilterFlags, object message, Object context)
        {
            if (Check_IsFiltering(pFilterFlags))
                return;

            string strMessageOut;
            _OnLogFormat(message, pFilterFlags, out strMessageOut);

            UnityEngine.Debug.LogWarning(strMessageOut, context);
        }
        #endregion

        /// <summary>
        /// 함수를 실행하며, [try catch]로 감쌉니다. catch 발생시 로그를 출력합니다.
        /// <para>리턴은 함수를 정상적으로 실행하면 true입니다.</para>
        /// </summary>
        /// <param name="TryFunc">실행할 함수</param>
        /// <param name="eLogType">catch시 출력할 로그 타입</param>
        static public bool TryExecute(System.Action TryFunc, LogType eLogType = LogType.Error)
        {
            try
            {
                TryFunc();
            }
            catch (System.Exception e)
            {
                switch (eLogType)
                {
                    case LogType.Log: Log(e); break;
                    case LogType.Warning: LogWarning(e); break;

                    case LogType.Error:
                    case LogType.Exception:
                        LogError(e);
                        break;
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// 함수를 실행하며, [try catch]로 감쌉니다. catch 발생시 로그를 출력합니다.
        /// <para>리턴은 함수를 정상적으로 실행하면 true입니다.</para>
        /// </summary>
        /// <param name="TryFunc">실행할 함수</param>
        /// <param name="pFilterFlags">출력할 필터 플래그입니다</param>
        /// <param name="eLogType">catch시 출력할 로그 타입</param>
        static public bool TryExecute(System.Action TryFunc, object pFilterFlags, LogType eLogType = LogType.Error)
        {
            try
            {
                TryFunc();
            }
            catch (System.Exception e)
            {
                switch (eLogType)
                {
                    case LogType.Log: Log(pFilterFlags, e); break;
                    case LogType.Warning: LogWarning(pFilterFlags, e); break;

                    case LogType.Error:
                    case LogType.Exception:
                        LogError(pFilterFlags, e);
                        break;
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// 함수를 실행하며, [try catch]로 감쌉니다. catch 발생시 로그를 출력합니다.
        /// <para>리턴은 함수를 정상적으로 실행하면 true입니다.</para>
        /// </summary>
        /// <param name="TryFunc">실행할 함수</param>
        /// <param name="pFilterFlags">출력할 필터 플래그입니다</param>
        /// <param name="eLogType">catch시 출력할 로그 타입</param>
        /// <returns></returns>
        static public bool TryExecute(System.Action TryFunc, object pFilterFlags, Object context, LogType eLogType = LogType.Error)
        {
            try
            {
                TryFunc();
            }
            catch (System.Exception e)
            {
                switch (eLogType)
                {
                    case LogType.Log: Log(pFilterFlags, e, context); break;
                    case LogType.Warning: LogWarning(pFilterFlags, e, context); break;

                    case LogType.Error:
                    case LogType.Exception:
                        LogError(pFilterFlags, e, context);
                        break;
                }

                return false;
            }

            return true;
        }

        #region UnityLog

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

        #endregion

        static bool Check_IsFiltering(object pFilterFlags)
        {
            if (pFilterFlags.Equals(strDefaultFlagName))
                return false;

            int iHashCode = pFilterFlags.GetHashCode();
            return (_iFilterFlags & iHashCode) != iHashCode;
        }

        static void LogFormat_Default(object pMessage, object pFilterFlags, out string strMessageResult)
        {
            string strColorHexCode = strDefaultColorHexCode;
            int iHashCode = pFilterFlags.GetHashCode();
            if (_mapColorHexCode.ContainsKey(iHashCode))
                strColorHexCode = _mapColorHexCode[iHashCode];

            strMessageResult = $"<color=#{strColorHexCode}>[{pFilterFlags}]</color> {pMessage}";
        }
    }
}
