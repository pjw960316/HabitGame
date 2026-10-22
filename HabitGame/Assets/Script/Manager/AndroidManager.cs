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

    // NOTE : Android 참조는 초기화 시 보관하지 않고 호출할 때 확보한다.

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Methods

    // NOTE : 반환된 Activity 래퍼는 호출자가 사용 후 해제한다.
    public AndroidJavaObject GetCurrentActivity()
    {
        return _androidContextHandler.GetCurrentActivity();
    }

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
    // Java 클래스 참조와 현재 Activity 참조를 확보한다.
    // TODO : 그 외 Android 호출 준비 책임은 실제 사용처를 보며 정한다.
    private sealed class AndroidContextHandler
    {
        // TODO : 디바이스 확인의 필요 범위와 비동기 처리 여부는 실제 호출 흐름을 정할 때 판단한다.

        public AndroidJavaObject GetCurrentActivity()
        {
            using (var unityPlayer = CreateJavaClass("com.unity3d.player.UnityPlayer"))
            {
                return unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            }
        }

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
