using CustomDebug;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public struct LogPrintInfo
{
    public ICustomLogType iLogType;
    public object pLogMessage;
    public Object pContextObject;

    public string strMember;
    public string strFilePath;
    public int iSourceLineNumber;

    public LogPrintInfo(ICustomLogType iLogType, object pLogMessage, Object pContextObject)
    {
        this.iLogType = iLogType;
        this.pLogMessage = pLogMessage;
        this.pContextObject = pContextObject;

        this.strMember = "";
        this.strFilePath = "";
        iSourceLineNumber = -1;
    }

    public LogPrintInfo(ICustomLogType iLogType, object pLogMessage, Object pContextObject, string strMember, string strFilePath, int iSourceLineNumber)
    {
        this.iLogType = iLogType;
        this.pLogMessage = pLogMessage;
        this.pContextObject = pContextObject;

        this.strMember = strMember;
        this.strFilePath = strFilePath;
        this.iSourceLineNumber = iSourceLineNumber;
    }
}

public interface ILogPrinter
{
    void ILogPrinter_OnPrintLog(string strFilterFlag, LogPrintInfo sLogPrintInfo, out string strMessageResult);
}

namespace CustomDebug
{
    public enum EDefaultLogFormatName // Enum의 Value와 타입 명이 일치해야 합니다.

    {
        DefaultLogFormat_Without_CallStack,
        DefaultLogFormat_With_CallStack,
    }

    public class DefaultLogFormat_Without_CallStack : ILogPrinter
    {
        /// <summary>
        /// 기본 로그 포멧
        /// </summary>
        public void ILogPrinter_OnPrintLog(string strFilterFlag, LogPrintInfo sLogPrintInfo, out string strMessageResult)
        {
            strMessageResult = $"<b>[{strFilterFlag}]</b> {sLogPrintInfo.pLogMessage}";
        }
    }

    public class DefaultLogFormat_With_CallStack : ILogPrinter
    {
        /// <summary>
        /// 기본 로그 포멧 - 파일
        /// </summary>
        public void ILogPrinter_OnPrintLog(string strFilterFlag, LogPrintInfo sLogPrintInfo, out string strMessageResult)
        {
            string strFilePath = sLogPrintInfo.strFilePath.Replace('\\', '/');
            string strFileName = Path.GetFileNameWithoutExtension(strFilePath);

            strMessageResult = $"[{strFilterFlag}] [{strFileName}.{sLogPrintInfo.strMember}.{sLogPrintInfo.iSourceLineNumber}] -  {sLogPrintInfo.pLogMessage}";
        }
    }
}
