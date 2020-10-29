using System;
using System.IO;
using CustomDebug;
using UnityEngine;
using Object = UnityEngine.Object;

public struct LogPrintInfo
{
    public ICustomLogType iLogType;
    public object pLogMessage;
    public Object pContextObject;

    public string strMember;
    public string strFilePath;
    public string strStackTrace;
    public int iSourceLineNumber;

    public LogPrintInfo(ICustomLogType iLogType, object pLogMessage, Object pContextObject)
    {
        this.iLogType = iLogType;
        this.pLogMessage = pLogMessage;
        this.pContextObject = pContextObject;

        this.strStackTrace = "";
        this.strMember = "";
        this.strFilePath = "";
        iSourceLineNumber = -1;
    }

    public LogPrintInfo(ICustomLogType iLogType, object pLogMessage, Object pContextObject, string strStackTrace, string strMember, string strFilePath, int iSourceLineNumber)
    {
        this.iLogType = iLogType;
        this.pLogMessage = pLogMessage;
        this.pContextObject = pContextObject;

        this.strStackTrace = strStackTrace;
        this.strMember = strMember;
        this.strFilePath = strFilePath;
        this.iSourceLineNumber = iSourceLineNumber;
    }
}

public interface ILogPrinter
{
    void ILogPrinter_OnPrintLog(LogType eLogType, string strFilterFlag, LogPrintInfo sLogPrintInfo, out string strMessageResult);
}

namespace CustomDebug
{
    /// <summary>
    /// 로그 포멧명
    /// </summary>
    public enum EDefaultLogFormatName // Enum의 Value와 타입 명이 일치해야 합니다.
    {
        /// <summary>
        /// 기본 로그 포멧
        /// <para>$"<b>[{strFilterFlag}]</b> {sLogPrintInfo.pLogMessage}"</para>
        /// </summary>
        DefaultLogFormat_Without_CallStack,

        /// <summary>
        /// 기본 로그 포멧 - 멤버 정보(함수명, 줄번호)
        /// <para>$"[{strFilterFlag}] [{strFileName}.{sLogPrintInfo.strMember}.{sLogPrintInfo.iSourceLineNumber}] -  {sLogPrintInfo.pLogMessage}"</para>
        /// </summary>
        DefaultLogFormat_Without_CallStack_OnlyMemberInfo,

        /// <summary>
        /// 기본 로그 포멧 - 멤버 정보(함수명, 줄번호) + 콜스택 포함용
        /// <para>$"[{strFilterFlag}] [{strFileName}.{sLogPrintInfo.strMember}.{sLogPrintInfo.iSourceLineNumber}] -  {sLogPrintInfo.pLogMessage}\n" + $"{sLogPrintInfo.strStackTrace}"</para>
        /// </summary>
        DefaultLogFormat_With_CallStack,
    }

    public class DefaultLogFormat_Without_CallStack : ILogPrinter
    {

        public void ILogPrinter_OnPrintLog(LogType eLogType, string strFilterFlag, LogPrintInfo sLogPrintInfo, out string strMessageResult)
        {
            strMessageResult = $"<b>[{strFilterFlag}]</b> {sLogPrintInfo.pLogMessage}";
        }
    }

    public class DefaultLogFormat_Without_CallStack_OnlyMemberInfo : ILogPrinter
    {
        public void ILogPrinter_OnPrintLog(LogType eLogType, string strFilterFlag, LogPrintInfo sLogPrintInfo, out string strMessageResult)
        {
            string strFilePath = sLogPrintInfo.strFilePath.Replace('\\', '/');
            string strFileName = Path.GetFileNameWithoutExtension(strFilePath);

            strMessageResult = $"[{strFilterFlag}] [{strFileName}.{sLogPrintInfo.strMember}.{sLogPrintInfo.iSourceLineNumber}] -  {sLogPrintInfo.pLogMessage}";
        }
    }

    public class DefaultLogFormat_With_CallStack : ILogPrinter
    {

        public void ILogPrinter_OnPrintLog(LogType eLogType, string strFilterFlag, LogPrintInfo sLogPrintInfo, out string strMessageResult)
        {
            string strFilePath = sLogPrintInfo.strFilePath.Replace('\\', '/');
            string strFileName = Path.GetFileNameWithoutExtension(strFilePath);

            strMessageResult = $"[{strFilterFlag}] [{strFileName}.{sLogPrintInfo.strMember}.{sLogPrintInfo.iSourceLineNumber}] -  {sLogPrintInfo.pLogMessage}\n" +
            $"{sLogPrintInfo.strStackTrace}";
        }
    }
}
