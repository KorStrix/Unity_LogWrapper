# 개요

유니티 Debug.Log를 기능을 추가한 래핑한 클래스입니다.

이 툴은 유니티에서 로컬 PC에서 Debug.Log를 빌드 한 후에도 Define Symbol로 로그 필터를 관리하기위한 툴입니다.

하단은 예시입니다.

```csharp
private void OnEnable()
{
    Wrapper.Debug.DoInit_PrintLog_FilterFlag(CLogType.Log);
    PrintLog(); 
    // 출력
    // log
    // log or warning
    // log or error


    Wrapper.Debug.DoInit_PrintLog_FilterFlag(CLogType.Log | CLogType.Warning);
    PrintLog();
    // 출력
    // log
    // error
    // log or warning
    // log and warning
    // log or error
}

private void PrintLog()
{
    Wrapper.Debug.Log(CLogType.Log, "test", this);
    Wrapper.Debug.Log(CLogType.Warning, "warning", this);
    Wrapper.Debug.Log(CLogType.Error, "error", this);
    
    Wrapper.Debug.Log(CLogType.Log | CLogType.Warning, "log or warning", this);
    Wrapper.Debug.Log(CLogType.Log & CLogType.Warning, "log and warning", this);
    Wrapper.Debug.Log(CLogType.Log | CLogType.Error, "log and error", this);
    Wrapper.Debug.Log(CLogType.Log & CLogType.Error, "log and error", this);
}
```

# 설치 방법

[링크](https://github.com/KorStrix/Unity_DevelopmentDocs/blob/master/GitHub/UnityPackage.md)를 참고바랍니다.

**Unity Editor 상단탭/Tools/LogWrapper Editor 메뉴가 뜨면 설치 성공!**

