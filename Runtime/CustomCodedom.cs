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
        public const string const_strListFieldName = "list";

        private static string const_strPrefix = @"
/*	============================================
 *	Author   			    : Strix
 *	Summary 		        : 
 *
 *  툴로 자동으로 생성되는 코드입니다.
 *  이 파일을 직접 수정하시면 나중에 툴로 생성할 때 날아갑니다.
   ============================================= */

using UnityEngine;
using System.Collections.Generic;

public partial class {0}
{
    // 로그타입 클래스 정의부
{1}


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init()
    {
        Debug.Log(""Log Init"");
        List<CustomDebug.ICustomLogType> {2} = new List<CustomDebug.ICustomLogType>();

        // 에디터일 경우
        if (Application.isEditor)
        {
            LogFilter_PerBranch pLocalBranch = LogFilter_PerBranch.Get_LogTypeEnable_FromPlayerPrefs(out bool bIsChange);
            if (bIsChange)
            {
                Debug.LogError($""Get LogTypeEnable FromPlayerPrefs Fail - Show {nameof(DebugWrapperEditor)}"");
                return;
            }

            {2}.AddRange(pLocalBranch.GetEnableLogType());
{4}
        }
        else
        {
{3}
        }

        Debug.Init_PrintLog_FilterFlag({2}.ToArray());
    }
}";

        enum ETextType
        {
            DefineClass,
            Branch,
            Local,

            MAX,
        }

        private Dictionary<ETextType, StringBuilder> mapBuilder = new Dictionary<ETextType, StringBuilder>();

        public CustomCodedom()
        {
            mapBuilder.Clear();
            for (int i = 0; i < (int)ETextType.MAX; i++)
                mapBuilder.Add((ETextType)i, new StringBuilder());
        }

        /// <summary>
        /// 
        /// </summary>
        public void DoAddClass(CustomLogType pFlag)
        {
            mapBuilder[ETextType.DefineClass].AppendLine(pFlag.ToCSharpCodeString());
        }

        public void DoAddBranch(LogFilter_PerBranch pBranch)
        {
            mapBuilder[ETextType.Branch].AppendLine(pBranch.ToCSharpCodeString(const_strListFieldName));
        }

        /// <summary>
        /// 
        /// </summary>
        public void DoExportCS(string strTypeName, string strFilePath_Absolute)
        {
            // string.Format이 안됨;
            //string strFileContent = string.Format(const_strPrefix, 
            //    nameof(CustomLogType),
            //    _strBuilder_Class.ToString());

            string strFileContent = const_strPrefix.
                Replace("{0}", strTypeName).
                Replace("{1}", mapBuilder[ETextType.DefineClass].ToString()).
                Replace("{2}", const_strListFieldName).

                Replace("{3}", mapBuilder[ETextType.Branch].ToString()).
                Replace("{4}", mapBuilder[ETextType.Local].ToString());


            File.WriteAllText($"{strFilePath_Absolute}.cs", strFileContent, Encoding.UTF8);
        }
    }
}
