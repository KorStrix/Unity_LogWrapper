using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomDebugLog_Tester : MonoBehaviour
{
    private void OnEnable()
    {
        Wrapper.Debug.DoInit_PrintLog_FilterFlag(CLogType.Log);
        PrintLog();


        //Wrapper.Debug.DoInit_PrintLog_FilterFlag(CLogType.Warning);
        //PrintLog();


        Wrapper.Debug.DoInit_PrintLog_FilterFlag(CLogType.Log, CLogType.Warning);
        PrintLog();
    }

    private void PrintLog()
    {
        Wrapper.Debug.Log(CLogType.Log | CLogType.Warning, "log or warning", this);
        Wrapper.Debug.Log(CLogType.Log & CLogType.Warning, "log and warning", this);

        // Case 1. 로그 타입이 Log일 때만 출력을 원할 경우 예측값
        // 프로그래머 예측 값 : log or warning만 출력
        // 실제 값 : log or warning만 출력

        // Case 2. 로그 타입이 Log, Warning일 때만 출력을 원할 경우 예측값
        // 프로그래머 예측 값 : log or warning 및 log and warning 출력
        // 실제 값: log or warning만 출력

        Wrapper.Debug.Log(CLogType.Log | CLogType.Error, "log or error", this);
        Wrapper.Debug.Log(CLogType.Log & CLogType.Error, "log and error", this);

        Wrapper.Debug.Log(CLogType.Log, "log", this);
        Wrapper.Debug.Log(CLogType.Warning, "warning", this);
        Wrapper.Debug.Log(CLogType.Error, "error", this);
    }
}
