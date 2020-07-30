using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wrapper;

public class CustomDebugLog_Tester : MonoBehaviour
{
    private void OnEnable()
    {
        Wrapper.Debug.Log(CustomLogTypeT.Debug, "Test");
        //LogTest("Default");

        //Wrapper.Debug.Set_PrintLog_FilterFlag(EFilter.InGame | EFilter.OutGame);
        //LogTest(EFilter.InGame | EFilter.OutGame);

        //pFactory.DoAdd_DebugFilter(EFilter.OutGame, Color.blue);
        //pFactory.DoAdd_DebugFilter(EFilter.Error, Color.red);
        //pFactory.DoAdd_DebugFilter(EFilter.OutGame | EFilter.Error, Color.magenta);
        //pFactory.DoAdd_DebugFilter(EFilter.None, Color.white);
        //Wrapper.Debug.Init_PrintLog_FilterFlag(pFactory);

        // Wrapper.Debug.Log(EFilter.OutGame | EFilter.Error, EFilter.OutGame | EFilter.Error, this);
    }

    //private void LogTest(object strTestCase)
    //{
    //    Wrapper.Debug.Log("Start : " + strTestCase);
    //    Wrapper.Debug.Log("");

    //    Wrapper.Debug.Log(EFilter.InGame, nameof(EFilter.InGame), this);
    //    Wrapper.Debug.LogWarning(EFilter.OutGame, nameof(EFilter.OutGame), this);
    //    Wrapper.Debug.LogError(EFilter.InGame | EFilter.Error, (EFilter.InGame | EFilter.Error).ToString(), this);

    //    Wrapper.Debug.LogWarning("Default", this);

    //    Wrapper.Debug.TryExecute(() => throw new System.Exception(), EFilter.InGame, this);
    //    Wrapper.Debug.TryExecute(() => throw new System.Exception(), EFilter.OutGame, this);
    //    Wrapper.Debug.TryExecute(() => throw new System.Exception(), EFilter.Warning, this);
    //    Wrapper.Debug.TryExecute(() => throw new System.Exception(), EFilter.Error, this);

    //    Wrapper.Debug.Log("Finish : " + strTestCase);
    //    Wrapper.Debug.Log("");
    //}
}
