
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
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init2()
    {
        Wrapper.Debug.DoSet_OnLogFormat_Default(CustomDebug.EDefaultLogFormatName.DefaultLogFormat_With_CallStack);
    }
}
