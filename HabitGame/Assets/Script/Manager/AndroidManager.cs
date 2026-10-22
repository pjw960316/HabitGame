using System;
using UnityEngine;

// NOTE : 책임 (회사)
// 1. 외부의 클래스들이 Android(Java)에 접근 할 수 있게 public으로 열어둔 클래스. 소통은 이 녀석과
// 2. Java method 호출을 위한 정보를 외부 클래스로 부터 받아야 할 책임이 있다. 이걸 JavaMethodHandler에게 전달한다.
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

    // TODO : Activity 참조의 초기화와 해제 시점은 실제 Context 전달 흐름을 구현할 때 정한다.

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Methods

    // NOTE : Java 클래스 참조는 호출 범위에서 해제하고, 전달받은 인자는 호출자가 관리한다.
    public void CallJavaMethod(string javaClassName, string methodName, params object[] args)
    {
        using (var javaClass = _androidContextHandler.CreateJavaClass(javaClassName))
        {
            _javaMethodHandler.CallJavaMethod(javaClass, methodName, args);
        }
    }

    public T CallJavaMethod<T>(string javaClassName, string methodName, params object[] args)
    {
        using (var javaClass = _androidContextHandler.CreateJavaClass(javaClassName))
        {
            return _javaMethodHandler.CallJavaMethod<T>(javaClass, methodName, args);
        }
    }

    #endregion

    // NOTE : 책임 (직원 1)
    // TODO : 이 녀석의 책임이 아직 애매함. 지금 이 녀석은 일단 자바 클래스 네임 예외처리 정도거든.
    private sealed class AndroidContextHandler
    {
        // TODO : 디바이스 확인의 필요 범위와 비동기 처리 여부는 실제 호출 흐름을 정할 때 판단한다.

        public AndroidJavaClass CreateJavaClass(string javaClassName)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return new AndroidJavaClass(javaClassName);
#else
            throw new PlatformNotSupportedException("Java static 메서드 호출은 Android 기기에서만 지원합니다.");
#endif
        }
    }

    // NOTE : 책임 (직원 2)
    // 준비된 참조와 요청자가 전달한 호출 정보를 사용하여 Java static 메서드를 호출한다.
    // 호출의 반환값과 예외를 AndroidManager에 전달한다.
    private sealed class JavaMethodHandler
    {
        public void CallJavaMethod(AndroidJavaClass javaClass, string methodName, params object[] args)
        {
            javaClass.CallStatic(methodName, args);
        }

        public T CallJavaMethod<T>(AndroidJavaClass javaClass, string methodName, params object[] args)
        {
            return javaClass.CallStatic<T>(methodName, args);
        }
    }
}
