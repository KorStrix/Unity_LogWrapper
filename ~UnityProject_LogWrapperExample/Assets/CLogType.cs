
/*	============================================
 *	Author   			    : Strix
 *	Summary 		        : 
 *
 *  툴로 자동으로 생성되는 코드입니다.
 *  이 파일을 직접 수정하시면 나중에 툴로 생성할 때 날아갑니다.
   ============================================= */

using UnityEngine;
using System.Collections.Generic;

public partial class CLogType
{
    // 로그타입 클래스 정의부
    public static CustomLogType a = new CustomLogType("a", 0, "FFFFFF");
    public static CustomLogType b = new CustomLogType("b", 0, "FFFFFF");
    public static CustomLogType c = new CustomLogType("c", 0, "FFFFFF");



    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init()
    {
        List<CustomDebug.ICustomLogType> list = new List<CustomDebug.ICustomLogType>();

        // 에디터일 경우
        if (Application.isEditor)
        {
            LogFilter_PerBranch pLocalBranch = LogFilter_PerBranch.Get_LogTypeEnable_FromPlayerPrefs(out bool bIsChange);
            if (bIsChange)
            {
                Debug.LogError($"Get LogTypeEnable FromPlayerPrefs Fail - Show {nameof(DebugWrapperEditor)}");
                return;
            }

            list.AddRange(pLocalBranch.GetEnableLogType());

        }
        else
        {
#if Alpha

            list.Add(b);
            list.Add(c);

#endif
#if Beta

            list.Add(a);
            list.Add(b);
            list.Add(c);

#endif

        }

        Debug.Init_PrintLog_FilterFlag(list.ToArray());
    }
}
