# 1. 개요

유니티 Debug.Log를 기능을 추가한 래핑한 클래스입니다.

이 툴은 유니티에서 로컬 PC에서 Debug.Log를 빌드 한 후에도 Define Symbol로 로그 필터를 관리하기위한 툴입니다.

하단은 주요 기능 및 예시입니다.

## 1. 어떤 로그 타입의 로그를 출력할지 필터링
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

![](https://github.com/KorStrix/Unity_LogWrapper/blob/master/ForGithub/Example.gif?raw=true)

## 2. Editor / Build Define Symbol에 의한 필터 관리

Editor에선 각자 컴퓨터에서 다른 로그 타입을 볼 수 있으며,

Build 후에는 Define Symbol에 따라 로그필터의 활성화 유무를 툴로 세팅할 수 있습니다.

## 3. EditorLogType(Log, Warning, Error 등)에 따라 로그 출력 형식 관리

Editor에선 어떤 파일에서 몇번째 줄에 어떤 콜스택으로 쌓였는지 알 필요가 없지만,

Build 후에는 원활한 디버깅을 위해 추적할 필요가 있습니다.

해서 다음과 같은 이미 구현된 기본 로그 출력 형식이 구현[(ILogPrinter.cs  코드링크 참고)](https://github.com/KorStrix/Unity_LogWrapper/blob/master/~UnityLogDLL_VSProject/ILogPrinter.cs) 되어 있으며, ILogPrinter를 상속받아 커스텀하게 구현할 수 있습니다.

# 2. 설치 방법

[링크](https://github.com/KorStrix/Unity_DevelopmentDocs/blob/master/GitHub/UnityPackage.md)를 참고바랍니다.

# 3. 사용 방법

## 3-1. 세팅 방법

1. Unity Editor 상단 탭 - Tools - LogWrapper Editor를 클릭합니다.
2. 기존에 Setting 파일이 없을 경우 Asset/Resource에 자동으로 Setting파일이 생성됩니다.
3. 여기서 로그의 타입이름과 해당 로그타입의 이름을 **출력할 CS파일 이름**을 설정합니다.
4. 로그 타입을 정의합니다.
5. Export 버튼을 눌러 로그타입이 담긴 cs파일을 생성합니다.

![](https://github.com/KorStrix/Unity_LogWrapper/blob/master/ForGithub/SettingExample.gif?raw=true)

**Unity 프로젝트에서 Assets/Resources/ 에 3번에서 설정한 .cs파일이 있으면 성공!**

