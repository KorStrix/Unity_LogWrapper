
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
    /// <summary>
    /// aa
    /// </summary>
    public static CustomLogType Log = new CustomLogType("Log", 1, "00FF24");

    /// <summary>
    /// bb
    /// </summary>
    public static CustomLogType Warning = new CustomLogType("Warning", 2, "FFD900");

    /// <summary>
    /// cc
    /// </summary>
    public static CustomLogType Error = new CustomLogType("Error", 4, "FF1500");




    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init()
    {
        Debug.Log("Log Init");
        List<CustomDebug.ICustomLogType> list = new List<CustomDebug.ICustomLogType>();

        // 에디터일 경우
        if (Application.isEditor)
        {
            LogFilter_PerBranch pLocalBranch = LogFilter_PerBranch.Get_LogTypeEnable_FromEditorPrefs(out bool bIsChange);
            pLocalBranch.pSetting = LogWrapperSetting.pCurrentSetting;

            if (bIsChange)
            {
                Debug.LogError($"Get LogTypeEnable FromPlayerPrefs Fail");
                return;
            }

            list.AddRange(pLocalBranch.GetEnableLogType());

        }
        else
        {
#if Alpha

            list.Add(asdf);
            list.Add(fdsa);
            list.Add(asddf);

#endif
#if Beta

            list.Add(asdf);
            list.Add(fdsa);
            list.Add(asddf);

#endif

        }

        Wrapper.Debug.DoInit_PrintLog_FilterFlag(list.ToArray());
    }
}
