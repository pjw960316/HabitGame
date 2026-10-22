
// NOTE : 책임 (회사)
// 기능별 클래스가 전달한 Java 호출 정보를 받아 직원에게 실행을 맡기는 공통 진입점이다.
// Android 호출 준비 -> Java 메서드 호출 -> 결과 및 예외 전달의 흐름을 조율한다.
// 기능별 요청 구성과 반환값의 의미 해석, 비동기 작업의 완료 판단은 요청자가 담당한다.
public sealed class AndroidManager : ManagerBase<AndroidManager>
{
    #region 1. Fields

    private readonly AndroidContextHandler _androidContextHandler = new();
    private readonly JavaMethodHandler _javaMethodHandler = new();

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    // TODO : 직원의 초기화와 Android 참조의 해제 시점은 실제 호출 흐름을 구현할 때 정한다.

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Methods

    // TODO : 요청자가 전달할 호출 정보의 형식과 공통 호출 메서드는 다음 단계에서 구현한다.

    #endregion

    // NOTE : 책임 (직원 1)
    // Android에서 Java 메서드를 호출할 수 있는 환경인지 확인한다.
    // 호출에 필요한 Activity와 Java 클래스 참조의 확보 및 수명 관리를 담당한다.
    private sealed class AndroidContextHandler
    {
    }

    // NOTE : 책임 (직원 2)
    // 준비된 참조와 요청자가 전달한 호출 정보를 사용하여 Java static 메서드를 호출한다.
    // 호출의 반환값과 예외를 AndroidManager에 전달한다.
    private sealed class JavaMethodHandler
    {
    }
}