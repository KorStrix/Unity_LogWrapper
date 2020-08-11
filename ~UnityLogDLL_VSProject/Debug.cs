using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using CustomDebug;
using System.Text;
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
        private static readonly LogType[] const_arrLogTypeAll = new LogType[]
        {
            LogType.Log,
            LogType.Warning,
            LogType.Error,
            LogType.Assert,
            LogType.Error
        };

        /// <summary>
        /// 플래그를 지정하지 않은 로그에 대한 플래그
        /// </summary>
        static DefaultLogType Default = DefaultLogType.Default;

        static Dictionary<string, string> _mapColorHexCode_ByString = new Dictionary<string, string>();
        static ulong _ulFilterFlags;

        private static Dictionary<LogType, ILogPrinter> _mapLogPrinter = new Dictionary<LogType, ILogPrinter>()
        {
            {LogType.Log, new DefaultLogFormat_Without_CallStack()},
            {LogType.Warning, new DefaultLogFormat_Without_CallStack()},
            {LogType.Error, new DefaultLogFormat_Without_CallStack()},
            {LogType.Assert, new DefaultLogFormat_Without_CallStack()},
            {LogType.Exception, new DefaultLogFormat_Without_CallStack()}
        };

        private static Dictionary<ulong, List<ILogLogic>> _mapOnPrintLog_LogicList = new Dictionary<ulong, List<ILogLogic>>();

        public static void DoClear_LogLogicList()
        {
            foreach (var listLogic in _mapOnPrintLog_LogicList.Values)
                listLogic.Clear();
            _mapOnPrintLog_LogicList.Clear();
        }

        public static void DoAdd_LogLogic(ILogLogic iLogic, params ICustomLogType[] arrExecuteLog)
        {
            ulong lFilterFlags = 0;
            foreach (var pLogType in arrExecuteLog)
                lFilterFlags |= pLogType.Number;

            if (_mapOnPrintLog_LogicList.TryGetValue(lFilterFlags, out var list) == false)
            {
                list = new List<ILogLogic>();
                _mapOnPrintLog_LogicList.Add(lFilterFlags, list);
            }

            list.Add(iLogic);
        }

        /// <summary>
        /// 출력할 로그의 필터를 지정합니다.
        /// </summary>
        public static void DoInit_PrintLog_FilterFlag(params ICustomLogType[] arrDebugFilter)
        {
            StringBuilder strBuilder_ForInitLog = new StringBuilder();
            _mapColorHexCode_ByString.Clear();

            _ulFilterFlags = 0;

            AddFilter(Default, strBuilder_ForInitLog);
            foreach (var pFilter in arrDebugFilter)
                AddFilter(pFilter, strBuilder_ForInitLog);

            UnityEngine.Debug.Log($"Log Init Count : {arrDebugFilter.Count()} - Show Flags\n" + strBuilder_ForInitLog);
        }

        /// <summary>
        /// 로그의 출력 포멧을 지정합니다.
        /// </summary>
        public static ILogPrinter DoSet_OnLogFormat_Default(EDefaultLogFormatName eName) => DoSet_OnLogFormat_Default(eName, const_arrLogTypeAll);

        /// <summary>
        /// 로그의 출력 포멧을 지정합니다.
        /// </summary>
        public static ILogPrinter DoSet_OnLogFormat_Default(EDefaultLogFormatName eName, params LogType[] arrLogType)
        {
            ILogPrinter iPrinter = null;
            switch (eName)
            {
                case EDefaultLogFormatName.DefaultLogFormat_Without_CallStack: iPrinter = new DefaultLogFormat_Without_CallStack(); break;
                case EDefaultLogFormatName.DefaultLogFormat_Without_CallStack_OnlyMemberInfo: iPrinter = new DefaultLogFormat_Without_CallStack_OnlyMemberInfo(); break;
                case EDefaultLogFormatName.DefaultLogFormat_With_CallStack: iPrinter = new DefaultLogFormat_With_CallStack(); break;
            }

            for (int i = 0; i < arrLogType.Length; i++)
                _mapLogPrinter[arrLogType[i]] = iPrinter;

            return iPrinter;
        }

        /// <summary>
        /// Default 외에 로그의 출력 포멧을 지정합니다.
        /// </summary>
        public static void DoSet_OnLogFormat_Custom(ILogPrinter iLogPrinter) => DoSet_OnLogFormat_Custom(iLogPrinter, const_arrLogTypeAll);

        /// <summary>
        /// Default 외에 로그의 출력 포멧을 지정합니다.
        /// </summary>
        public static void DoSet_OnLogFormat_Custom(ILogPrinter iLogPrinter, params LogType[] arrLogType)
        {
            for (int i = 0; i < arrLogType.Length; i++)
                _mapLogPrinter[arrLogType[i]] = iLogPrinter;
        }


        /// <summary>
        /// 함수를 실행하며, [try catch]로 감쌉니다. catch 발생시 로그를 출력합니다.
        /// <para>리턴은 함수를 정상적으로 실행하면 true입니다.</para>
        /// </summary>
        /// <param name="TryFunc">실행할 함수</param>
        /// <param name="eLogType">catch시 출력할 로그 타입</param>
        public static bool TryExecute(Action TryFunc, LogType eLogType = LogType.Error)
        {
            return TryExecute(TryFunc, Default, eLogType);
        }

        /// <summary>
        /// 함수를 실행하며, [try catch]로 감쌉니다. catch 발생시 로그를 출력합니다.
        /// <para>리턴은 함수를 정상적으로 실행하면 true입니다.</para>
        /// </summary>
        /// <param name="TryFunc">실행할 함수</param>
        /// <param name="pFilterFlags">출력할 필터 플래그입니다</param>
        /// <param name="eLogType">catch시 출력할 로그 타입</param>
        public static bool TryExecute(System.Action TryFunc, ICustomLogType pFilterFlags, LogType eLogType = LogType.Error)
        {
            try
            {
                TryFunc();
            }
            catch (Exception e)
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
        public static bool TryExecute(System.Action TryFunc, ICustomLogType pFilterFlags, Object context, LogType eLogType = LogType.Error)
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
                    case LogType.Error: LogError(pFilterFlags, e, context); break;
                    case LogType.Exception: LogException(e); break;
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// 현재 Wrapper에 필터가 들어있는지 유무
        /// </summary>
        public static bool Check_IsContainFilter(ICustomLogType pFilterFlags)
        {
            return Check_IsContainFilter(_ulFilterFlags, pFilterFlags);
        }

        /// <summary>
        /// 현재 Wrapper에 필터가 들어있는지 유무
        /// </summary>
        static bool Check_IsContainFilter(ulong ulFilterFlags, ICustomLogType pFilterFlags)
        {
            if (pFilterFlags.Equals(Default))
                return true;

            ulong iHashCode = pFilterFlags.Number;
            // UnityEngine.Debug.Log($"LogTypeName : {pFilterFlags.LogTypeName} // _ulFilterFlags : {_ulFilterFlags} // iHashCode : {iHashCode} // (_ulFilterFlags & iHashCode) : {(_ulFilterFlags & iHashCode)} // pFilterFlags.eOperatorType: {pFilterFlags.eOperatorType}");

            if (pFilterFlags.eOperatorType == EOperatorType.AND)
                return (ulFilterFlags & iHashCode) == iHashCode;
            else
                return (ulFilterFlags & iHashCode) != 0;
        }


        private static void AddFilter(ICustomLogType pFilter, StringBuilder strBuilder_ForInitLog)
        {
            string strLogTypeName = pFilter.LogTypeName;
            string strHexCode = pFilter.ColorHexCode;
            strHexCode = strHexCode.Substring(0, 6);

            // int iHashCode = pFilterFlag.GetHashCode();
            ulong iHashCode = pFilter.Number;
            _ulFilterFlags |= iHashCode;
            _mapColorHexCode_ByString.Add(strLogTypeName, strHexCode);

            strBuilder_ForInitLog.Append($"<color=#{strHexCode}>[{strLogTypeName}]({iHashCode})</color> /");
        }

        private static void PrintLog(ICustomLogType pFilterFlags, string strStacktrace, object message, Object context, LogType eLogType, string strMemberName, string strFilePath, int iSourceLineNumber)
        {
            if (_mapLogPrinter.TryGetValue(eLogType, out ILogPrinter pPrinter) == false)
                return;



            string strFilterFlag = pFilterFlags.LogTypeName;

            foreach (var pColorString in _mapColorHexCode_ByString.OrderBy(p => p.Key.Length * -1))
            {
                string strKey = pColorString.Key;
                string strPattern = $"(?={strKey}([^a-z|A-Z|<|_]|))({strKey})";

                strFilterFlag = Regex.Replace(strFilterFlag, strPattern, $"<color=#{pColorString.Value}>{strKey}</color>");
            }

             pPrinter.ILogPrinter_OnPrintLog(eLogType, strFilterFlag, new LogPrintInfo(pFilterFlags, message, context,
                strStacktrace, strMemberName, strFilePath, iSourceLineNumber), out var strMessageOut);

            var arrExecuteLogic = _mapOnPrintLog_LogicList.Where(p => Check_IsContainFilter(p.Key, pFilterFlags)).Select(p => p.Value);

            foreach (List<ILogLogic> listLogic in arrExecuteLogic)
                listLogic.ForEach(p => p.ILogLogic_OnExecuteLogic(pFilterFlags, strMessageOut, context));

            switch (eLogType)
            {
                case LogType.Log: UnityEngine.Debug.Log(strMessageOut, context); break;
                case LogType.Warning: UnityEngine.Debug.LogWarning(strMessageOut, context); break;
                case LogType.Error: UnityEngine.Debug.LogError(strMessageOut, context); break;
            }
        }
    }
}
