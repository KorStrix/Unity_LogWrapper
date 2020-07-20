using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Wrapper
{
    /// <summary>
    /// cs 파일을 만드는 클래스입니다.
    /// <para>Unity .net 3.5에선 Codedom을 지원하지 않기 때문에 Custom으로 만들었습니다. </para>
    /// </summary>
    public class CustomCodedom
    {
        private static string const_strClass = @"
/// 툴로 자동으로 생성되는 코드입니다.
/// 이 파일을 직접 수정하시면 나중에 툴로 생성할 때 날아갑니다.

namespace {0}
{
    public partial class {1}
    {
{2}
    }
}";

        private StringBuilder _strBuilder = new StringBuilder();

        public CustomCodedom()
        {
            _strBuilder.Length = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public void DoAddClass(CustomLogType pFlag)
        {
            _strBuilder.AppendLine("        " + pFlag.ToCSharpCodeString());
        }

        /// <summary>
        /// 
        /// </summary>
        public void DoExportCS(string strFilePath_Absolute)
        {
            // string.Format이 안됨;
            //string strFileContent = string.Format(const_strClass, 
            //    nameof(CustomLogType),
            //    _strBuilder.ToString());

            string strFileContent = const_strClass.
                Replace("{0}", nameof(Wrapper)).
                Replace("{1}", nameof(CustomLogType)).
                Replace("{2}", _strBuilder.ToString());


            File.WriteAllText($"{strFilePath_Absolute}.cs", strFileContent, Encoding.UTF8);
        }
    }
}
