using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomDebugLog_Tester : MonoBehaviour
{
    [System.Flags]
    enum EFilter
    {
        None = 0, 
        InGame = 1 << 0,
        OutGame = 1 << 1,

        Warning = 1 << 2,
        Error = 1 << 3,
    }
    private void OnEnable()
    {
        //LogTest("Default");

        //Wrapper.Debug.Set_PrintLog_FilterFlag(EFilter.InGame | EFilter.OutGame);
        //LogTest(EFilter.InGame | EFilter.OutGame);

        Wrapper.Debug.DebugFilterInfo pFilter_Programmer_2 = new Wrapper.Debug.DebugFilterInfo(EFilter.OutGame, "ff0000");
        Wrapper.Debug.DebugFilterInfo pFilter_Somthing = new Wrapper.Debug.DebugFilterInfo(EFilter.Error, "ffff00");
        Wrapper.Debug.DebugFilterInfo pFilter_Somthing2 = new Wrapper.Debug.DebugFilterInfo(EFilter.OutGame | EFilter.Error, "00ff00");

        Wrapper.Debug.DebugFilterInfo pFilter_Default = new Wrapper.Debug.DebugFilterInfo("Default", "ffffff");
        Wrapper.Debug.Set_PrintLog_FilterFlag(pFilter_Programmer_2, pFilter_Somthing, pFilter_Somthing2, pFilter_Default);
        Wrapper.Debug.Log(EFilter.OutGame | EFilter.Error, EFilter.OutGame | EFilter.Error, this);

        // LogTest("DebugFilterInfo");
    }

    private void LogTest(object strTestCase)
    {
        Wrapper.Debug.Log("Start : " + strTestCase);
        Wrapper.Debug.Log("");

        Wrapper.Debug.Log(EFilter.InGame, nameof(EFilter.InGame), this);
        Wrapper.Debug.LogWarning(EFilter.OutGame, nameof(EFilter.OutGame), this);
        Wrapper.Debug.LogError(EFilter.InGame | EFilter.Error, (EFilter.InGame | EFilter.Error).ToString(), this);

        Wrapper.Debug.LogWarning("Default", this);

        Wrapper.Debug.TryExecute(() => throw new System.Exception(), EFilter.InGame, this);
        Wrapper.Debug.TryExecute(() => throw new System.Exception(), EFilter.OutGame, this);
        Wrapper.Debug.TryExecute(() => throw new System.Exception(), EFilter.Warning, this);
        Wrapper.Debug.TryExecute(() => throw new System.Exception(), EFilter.Error, this);

        Wrapper.Debug.Log("Finish : " + strTestCase);
        Wrapper.Debug.Log("");
    }
}
