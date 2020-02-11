using UnityEngine;
using System.Collections.Generic;

using Object = UnityEngine.Object;
using System.Linq;

#pragma warning disable CS0419 // cref 특성에 모호한 참조가 있음
#pragma warning disable CS1573 // 매개 변수와 짝이 맞는 매개 변수 태그가 XML 주석에 없습니다. 다른 매개 변수는 짝이 맞는 태그가 있습니다.

namespace Wrapper
{
    /// <summary>
    /// <see cref="UnityEngine.Debug"/>를 래핑한 클래스입니다.
    /// </summary>
    public static class Debug
    {
        /// <summary>
        /// 디폴트 색상 코드
        /// </summary>
        public const string const_strDefaultColorHexCode = "008000";

        /// <summary>
        /// 디버그 필터당 정보
        /// </summary>
        public struct DebugFilterInfo
        {
            /// <summary>
            /// 디버그 필터 플래그
            /// </summary>
            public object pFilterFlag;

            /// <summary>
            /// 디버그 로그의 색상값
            /// </summary>
            public string strColorHexCode;

            /// <summary>
            /// 디버그 필터당 정보 생성 (보다 디테일한 필터 설정을)
            /// </summary>
            /// <param name="pFilterFlag">디버그 필터 플래그</param>
            /// <param name="strColorHexCode">로그의 색상값 Ex)ffffff(RGB)</param>
            public DebugFilterInfo(object pFilterFlag, string strColorHexCode)
            {
                this.pFilterFlag = pFilterFlag; this.strColorHexCode = strColorHexCode;
            }
        }

#pragma warning disable CS1591 // 공개된 형식 또는 멤버에 대한 XML 주석이 없습니다.
        public delegate void PrintLogFormat(object pMessage, object pFilterFlags, out string strMessageResult);
#pragma warning restore CS1591 // 공개된 형식 또는 멤버에 대한 XML 주석이 없습니다.

        /// <summary>
        /// 로그에 출력하는 색상
        /// <para>Ex) 008000</para>
        /// <para>RGBA</para>
        /// </summary>
        static public string strDefaultColorHexCode = const_strDefaultColorHexCode;

        /// <summary>
        /// 플래그를 지정하지 않은 로그에 대한 플래그
        /// </summary>
        static public string strDefaultFlagName = "Default";

        static Dictionary<string, string> _mapColorHexCode_ByString = new Dictionary<string, string>();
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
            _mapColorHexCode_ByString.Clear();

            _iFilterFlags = 0;
            for (int i = 0; i < arrPrintFilterFlag.Length; i++)
            {
                object pFilterFlag = arrPrintFilterFlag[i].pFilterFlag;
                string strHexCode = arrPrintFilterFlag[i].strColorHexCode;
                strHexCode = strHexCode.Substring(0, 6);

                if (pFilterFlag.Equals(strDefaultFlagName))
                {
                    strDefaultColorHexCode = strHexCode;
                }
                else
                {
                    int iHashCode = pFilterFlag.GetHashCode();
                    _iFilterFlags |= iHashCode;
                    _mapColorHexCode_ByString.Add(pFilterFlag.ToString(), strHexCode);
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
            if (Check_IsContainFilter(pFilterFlags) == false)
                return;

            string strMessageOut;
            _OnLogFormat(message, pFilterFlags, out strMessageOut);

            UnityEngine.Debug.Log(strMessageOut, context);
        }

        static private void LogError_Custom(object pFilterFlags, object message, Object context)
        {
            // 에러는 반드시 출력
            //if (Check_IsFiltering(pFilterFlags))
            //    return;

            string strMessageOut;
            _OnLogFormat(message, pFilterFlags, out strMessageOut);

            UnityEngine.Debug.LogError(strMessageOut, context);
        }

        static private void LogWarning_Custom(object pFilterFlags, object message, Object context)
        {
            if (Check_IsContainFilter(pFilterFlags) == false)
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
            return TryExecute(TryFunc, null, eLogType);
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

        /// <summary>
        /// 현재 Wrapper에 필터가 들어있는지 유무
        /// </summary>
        public static bool Check_IsContainFilter(object pFilterFlags)
        {
            if (pFilterFlags.Equals(strDefaultFlagName))
                return true;

            int iHashCode = pFilterFlags.GetHashCode();
            return (_iFilterFlags & iHashCode) == iHashCode;
        }

#pragma warning disable CS1591 // 공개된 형식 또는 멤버에 대한 XML 주석이 없습니다.
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
#pragma warning restore CS1591 // 공개된 형식 또는 멤버에 대한 XML 주석이 없습니다.

        static void LogFormat_Default(object pMessage, object pFilterFlags, out string strMessageResult)
        {
            string strColorHexCode = strDefaultColorHexCode;
            int iHashCode = pFilterFlags.GetHashCode();

            string strFilterFlag = pFilterFlags.ToString();
            foreach(var pColorString in _mapColorHexCode_ByString)
                strFilterFlag = strFilterFlag.Replace(pColorString.Key, $"<color=#{pColorString.Value}>{pColorString.Key}</color>");

            strMessageResult = $"<b>[{strFilterFlag}]</b> {pMessage}";
        }
    }
}
