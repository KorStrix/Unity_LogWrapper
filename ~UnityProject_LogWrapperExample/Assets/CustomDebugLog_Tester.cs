using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomDebugLog_Tester : MonoBehaviour
{
    private void OnEnable()
    {
        Wrapper.Debug.Log(CLogType.Log | CLogType.Warning, "log or warning", this);
        Wrapper.Debug.Log(CLogType.Log & CLogType.Warning, "log and warning", this);
        Wrapper.Debug.Log(CLogType.Log, "test", this);
        Wrapper.Debug.Log(CLogType.Warning, "warning", this);
        Wrapper.Debug.Log(CLogType.Error, "error", this);

        Wrapper.Debug.Log(CLogType.Log & CLogType.Error, "log and error", this);
    }
}
