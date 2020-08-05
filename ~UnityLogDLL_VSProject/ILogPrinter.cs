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
    void ILogPrinter_OnPrintLog(IEnumerable<KeyValuePair<string, string>> arrHexCode_ByString, LogPrintInfo sLogPrintInfo, out string strMessageResult);
}

namespace CustomDebug
{
    public enum EDefaultLogFormatName
    {
        DefaultLogFormat_Editor,
        DefaultLogFormat_Build,
    }

    public class DefaultLogFormat_Editor : ILogPrinter
    {
        /// <summary>
        /// 기본 로그 포멧
        /// </summary>
        public void ILogPrinter_OnPrintLog(IEnumerable<KeyValuePair<string, string>> arrHexCode_ByString, LogPrintInfo sLogPrintInfo, out string strMessageResult)
        {
            string strFilterFlag = sLogPrintInfo.iLogType.LogTypeName;

            foreach (var pColorString in arrHexCode_ByString)
            {
                string strKey = pColorString.Key;
                string strPattern = $"(?={strKey}([^a-z|A-Z|<|_]|))({strKey})";

                strFilterFlag = Regex.Replace(strFilterFlag, strPattern, $"<color=#{pColorString.Value}>{strKey}</color>");
            }

            strMessageResult = $"<b>[{strFilterFlag}]</b> {sLogPrintInfo.pLogMessage}";
        }
    }

    public class DefaultLogFormat_Build : ILogPrinter
    {
        /// <summary>
        /// 기본 로그 포멧
        /// </summary>
        public void ILogPrinter_OnPrintLog(IEnumerable<KeyValuePair<string, string>> arrHexCode_ByString, LogPrintInfo sLogPrintInfo, out string strMessageResult)
        {
            string strFilterFlag = sLogPrintInfo.iLogType.LogTypeName;
            string strFilePath = sLogPrintInfo.strFilePath.Replace('\\', '/');
            string strFileName = Path.GetFileNameWithoutExtension(strFilePath);

            foreach (var pColorString in arrHexCode_ByString)
            {
                string strKey = pColorString.Key;
                string strPattern = $"(?={strKey}([^a-z|A-Z|<|_]|))({strKey})";

                strFilterFlag = Regex.Replace(strFilterFlag, strPattern, $"<color=#{pColorString.Value}>{strKey}</color>");
            }

            strMessageResult = $"[{strFilterFlag}] [{strFileName}.{sLogPrintInfo.strMember}.{sLogPrintInfo.iSourceLineNumber}] -  {sLogPrintInfo.pLogMessage}";
        }
    }
}
