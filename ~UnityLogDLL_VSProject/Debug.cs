using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Object = UnityEngine.Object;

#pragma warning disable CS0419 // cref 특성에 모호한 참조가 있음
#pragma warning disable CS1573 // 매개 변수와 짝이 맞는 매개 변수 태그가 XML 주석에 없습니다. 다른 매개 변수는 짝이 맞는 태그가 있습니다.

namespace Wrapper
{
    /// <summary>
    /// <see cref="UnityEngine.Debug"/>를 래핑한 클래스입니다.
    /// </summary>
    public static partial class Debug
    {
        /// <summary>
        /// 디폴트 색상 코드
        /// </summary>
        public const string const_strDefaultColorHexCode = "008000";

#pragma warning disable CS1591 // 공개된 형식 또는 멤버에 대한 XML 주석이 없습니다.
        public delegate void PrintLogFormat(object pFilterFlags, object pMessage, out string strMessageResult);
#pragma warning restore CS1591 // 공개된 형식 또는 멤버에 대한 XML 주석이 없습니다.

        /// <summary>
        /// 로그에 출력하는 색상
        /// <para>Ex) 008000</para>
        /// <para>RGB</para>
        /// </summary>
        public static string strDefaultColorHexCode = const_strDefaultColorHexCode;

        /// <summary>
        /// 플래그를 지정하지 않은 로그에 대한 플래그
        /// </summary>
        public static string strDefaultFlagName = "Default";

        static Dictionary<string, string> _mapColorHexCode_ByString = new Dictionary<string, string>();
        static int _iFilterFlags;
        static PrintLogFormat _OnLogFormat = LogFormat_Default;



        /// <summary>
        /// 출력할 로그의 필터 <see cref="System.FlagsAttribute"/>를 지정합니다.
        /// </summary>
        public static void Init_PrintLog_FilterFlag(IEnumerable<CustomLogType> arrDebugFilter)
        {
            _mapColorHexCode_ByString.Clear();

            _iFilterFlags = 0;
            foreach(var pFilter in arrDebugFilter)
            {
                object pFilterFlag = pFilter.strFlagName;
                string strHexCode = pFilter.strColorHexCode;
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
        public static void Set_OnLogFormat(PrintLogFormat OnLogFormat)
        {
            _OnLogFormat = OnLogFormat;
        }



        /// <summary>
        /// <see cref="UnityEngine.Debug.Log(object)"/>를 출력합니다.
        /// <para><see cref="Init_PrintLog_FilterFlag"/>에서 세팅한 플래그가 아니면 출력하지 않습니다.</para>
        /// </summary>
        /// <param name="pFilterFlags">출력할 필터 플래그입니다</param>
        /// <param name="message">로그 메시지</param>
        public static void Log(object pFilterFlags, object message)
        {
            Log_Custom(pFilterFlags, message, null);
        }

        /// <summary>
        /// <see cref="UnityEngine.Debug.Log(object, Object)"/>를 출력합니다.
        /// <para><see cref="Init_PrintLog_FilterFlag"/>에서 세팅한 플래그가 아니면 출력하지 않습니다.</para>
        /// </summary>
        /// <param name="pFilterFlags">출력할 필터 플래그입니다</param>
        /// <param name="message">로그 메시지</param>
        /// <param name="context">유니티 오브젝트</param>
        public static void Log(object pFilterFlags, object message, Object context)
        {
            Log_Custom(pFilterFlags, message, context);
        }

        /// <summary>
        /// <see cref="UnityEngine.Debug.LogError(object)"/>를 출력합니다.
        /// <para><see cref="Init_PrintLog_FilterFlag"/>에서 세팅한 플래그가 아니면 출력하지 않습니다.</para>
        /// </summary>
        /// <param name="pFilterFlags">출력할 필터 플래그입니다</param>
        /// <param name="message">로그 에러 메시지</param>
        public static void LogError(object pFilterFlags, object message)
        {
            LogError_Custom(pFilterFlags, message, null);
        }

        /// <summary>
        /// <see cref="UnityEngine.Debug.LogError(object, Object)"/>를 출력합니다.
        /// <para><see cref="Init_PrintLog_FilterFlag"/>에서 세팅한 플래그가 아니면 출력하지 않습니다.</para>
        /// </summary>
        /// <param name="pFilterFlags">출력할 필터 플래그입니다</param>
        /// <param name="message">로그 에러 메시지</param>
        public static void LogError(object pFilterFlags, object message, Object context)
        {
            LogError_Custom(pFilterFlags, message, context);
        }

        /// <summary>
        /// <see cref="UnityEngine.Debug.LogWarning(object)"/>를 출력합니다.
        /// <para><see cref="Init_PrintLog_FilterFlag"/>에서 세팅한 플래그가 아니면 출력하지 않습니다.</para>
        /// </summary>
        /// <param name="pFilterFlags">출력할 필터 플래그입니다</param>
        /// <param name="message">로그 에러 메시지</param>
        public static void LogWarning(object pFilterFlags, object message)
        {
            LogWarning_Custom(pFilterFlags, message, null);
        }

        /// <summary>
        /// <see cref="UnityEngine.Debug.LogWarning(object, Object)"/>를 출력합니다.
        /// <para><see cref="Init_PrintLog_FilterFlag"/>에서 세팅한 플래그가 아니면 출력하지 않습니다.</para>
        /// </summary>
        /// <param name="pFilterFlags">출력할 필터 플래그입니다</param>
        /// <param name="message">로그 에러 메시지</param>
        public static void LogWarning(object pFilterFlags, object message, Object context)
        {
            LogWarning_Custom(pFilterFlags, message, context);
        }

        #region LogWrapping
        private static void Log_Custom(object pFilterFlags, object message, Object context)
        {
            if (Check_IsContainFilter(pFilterFlags) == false)
                return;

            _OnLogFormat(pFilterFlags, message, out var strMessageOut);

            UnityEngine.Debug.Log(strMessageOut, context);
        }

        private static void LogError_Custom(object pFilterFlags, object message, Object context)
        {
            // 에러는 반드시 출력
            //if (Check_IsFiltering(pFilterFlags))
            //    return;

            _OnLogFormat(pFilterFlags, message, out var strMessageOut);

            UnityEngine.Debug.LogError(strMessageOut, context);
        }

        private static void LogWarning_Custom(object pFilterFlags, object message, Object context)
        {
            if (Check_IsContainFilter(pFilterFlags) == false)
                return;

            _OnLogFormat(pFilterFlags, message, out var strMessageOut);

            UnityEngine.Debug.LogWarning(strMessageOut, context);
        }
        #endregion

        /// <summary>
        /// 함수를 실행하며, [try catch]로 감쌉니다. catch 발생시 로그를 출력합니다.
        /// <para>리턴은 함수를 정상적으로 실행하면 true입니다.</para>
        /// </summary>
        /// <param name="TryFunc">실행할 함수</param>
        /// <param name="eLogType">catch시 출력할 로그 타입</param>
        public static bool TryExecute(System.Action TryFunc, UnityEngine.LogType eLogType = UnityEngine.LogType.Error)
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
        public static bool TryExecute(System.Action TryFunc, object pFilterFlags, UnityEngine.LogType eLogType = UnityEngine.LogType.Error)
        {
            try
            {
                TryFunc();
            }
            catch (System.Exception e)
            {
                switch (eLogType)
                {
                    case UnityEngine.LogType.Log: Log(pFilterFlags, e); break;
                    case UnityEngine.LogType.Warning: LogWarning(pFilterFlags, e); break;

                    case UnityEngine.LogType.Error:
                    case UnityEngine.LogType.Exception:
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
        public static bool TryExecute(System.Action TryFunc, object pFilterFlags, Object context, UnityEngine.LogType eLogType = UnityEngine.LogType.Error)
        {
            try
            {
                TryFunc();
            }
            catch (System.Exception e)
            {
                switch (eLogType)
                {
                    case UnityEngine.LogType.Log: Log(pFilterFlags, e, context); break;
                    case UnityEngine.LogType.Warning: LogWarning(pFilterFlags, e, context); break;

                    case UnityEngine.LogType.Error:
                    case UnityEngine.LogType.Exception:
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

        /// <summary>
        /// 기본 로그 포멧
        /// </summary>
        public static void LogFormat_Default(object pFilterFlags, object pMessage, out string strMessageResult)
        {
            string strFilterFlag = pFilterFlags.ToString();

            // 필터 길이가 긴 순부터 체크
            var arrHexCode = _mapColorHexCode_ByString.OrderBy(p => p.Key.Length * -1);
            foreach (var pColorString in arrHexCode)
            {
                string strKey = pColorString.Key;
                string strPattern = $"(?={strKey}([^a-z|A-Z|<|_]|($)))({strKey})";

                strFilterFlag = Regex.Replace(strFilterFlag, strPattern, $"<color=#{pColorString.Value}>{strKey}</color>");
            }

            strMessageResult = $"<b>[{strFilterFlag}]</b> {pMessage}";
        }
    }
}
