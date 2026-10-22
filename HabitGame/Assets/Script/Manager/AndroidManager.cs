// NOTE
// System 지우지 말아야 한다.
using System;
using Unity.Notifications.Android;
using UnityEngine;

// TODO: Android 연동 시스템 설계 방향. 실제 책임 분리는 다음 작업에서 진행한다.
// 1. AndroidManager: 게임의 Android 기능 요청을 받고 담당 객체에 위임하는 진입점이다.
// 2. 연결 담당: 현재 Activity와 Java 클래스에 접근하는 공통 준비를 맡는다.
// 3. 호출 담당: 준비된 참조로 Android 플러그인의 static 메서드를 호출하고 결과를 반환한다.
// 여기서 연결은 Unity C#에서 Android 객체와 클래스에 접근할 수 있도록 준비하는 것을 뜻한다.
// 다음 구현에서 연결/호출 담당의 이름과 Java 참조의 소유 및 해제 시점을 결정한다.
public sealed class AndroidManager : ManagerBase<AndroidManager>
{
    #region 1. Fields

    private const int WATER_PRACTICE_NOTIFICATION_ID = 1002;
    private const string WATER_PRACTICE_CHANNEL_ID = "HabitWaterPractice";

    private readonly AndroidPracticeBridge _androidPracticeBridge = new();

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    public override void Initialize()
    {
        TestAndroid();
        TestWaterNotificationResponse();
    }

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Methods

    public AndroidPracticeSnapshot GetAndroidPracticeSnapshot()
    {
        return _androidPracticeBridge.GetSnapshot();
    }


    private void TestAndroid()
    {
        var snapshot = GetAndroidPracticeSnapshot();
        
        // 로그 찍어보자.
        Debug.Log(snapshot);
    }

    private void TestWaterNotificationResponse()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            // 이전에 [예]를 누른 기록을 먼저 읽고, 새 학습용 알림을 표시합니다.
            LogLastWaterConfirmation();
            ShowWaterNotificationPractice();
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
        }
#endif
    }

    private void LogLastWaterConfirmation()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        var confirmedAt = _androidPracticeBridge.GetLastWaterConfirmedAt();
        if (confirmedAt == 0)
        {
            Debug.Log("WaterNotificationPractice | 아직 [예]를 누른 기록이 없습니다.");
            return;
        }

        var localTime = DateTimeOffset.FromUnixTimeMilliseconds(confirmedAt).ToLocalTime();
        Debug.Log($"WaterNotificationPractice | 마지막 [예] 클릭: {localTime:yyyy-MM-dd HH:mm:ss zzz}");
#endif
    }

    public void ShowWaterNotificationPractice()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        var channel = new AndroidNotificationChannel(WATER_PRACTICE_CHANNEL_ID, "물 알림 학습", "알림 버튼과 날짜 저장을 확인하는 학습용 채널", Importance.Default);
        AndroidNotificationCenter.RegisterNotificationChannel(channel);

        // 예약을 기다리지 않고 즉시 표시하는 학습용 알림입니다.
        var notification = new AndroidNotification("[학습] 물 500mL 드세요", "드셨습니까? 아래 [예] 버튼을 눌러 주세요.", DateTime.Now);
        notification.ShouldAutoCancel = true;

        using (var builder = AndroidNotificationCenter.CreateNotificationBuilder(WATER_PRACTICE_NOTIFICATION_ID, notification, WATER_PRACTICE_CHANNEL_ID))
        {
            if (builder == null)
            {
                Debug.LogWarning("WaterNotificationPractice | Android 알림을 초기화하지 못했습니다.");
                return;
            }

            _androidPracticeBridge.AddWaterConfirmationButton(builder, WATER_PRACTICE_NOTIFICATION_ID);
            AndroidNotificationCenter.SendNotification(builder);
        }
#endif
    }

    #endregion

    private sealed class AndroidPracticeBridge
    {
        private const string WATER_RECEIVER_CLASS_NAME = "com.habitgame.notificationpractice.WaterNotificationReceiver";

        public void AddWaterConfirmationButton(AndroidJavaObject builder, int notificationId)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            using (var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (var currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
            using (var receiverClass = new AndroidJavaClass(WATER_RECEIVER_CLASS_NAME))
            {
                // Java 파일의 addConfirmButton()을 호출해 버튼과 수신기를 연결합니다.
                receiverClass.CallStatic("addConfirmButton", currentActivity, builder, notificationId);
            }
#endif
        }

        public long GetLastWaterConfirmedAt()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            using (var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (var currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
            using (var receiverClass = new AndroidJavaClass(WATER_RECEIVER_CLASS_NAME))
            {
                return receiverClass.CallStatic<long>("getLastConfirmedAt", currentActivity);
            }
#else
            return 0;
#endif
        }

        public AndroidPracticeSnapshot GetSnapshot()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            using (var buildClass = new AndroidJavaClass("android.os.Build"))
            using (var versionClass = new AndroidJavaClass("android.os.Build$VERSION"))
            using (var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (var currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
            using (var activityClass = currentActivity.Call<AndroidJavaObject>("getClass"))
            {
                return new AndroidPracticeSnapshot(
                    "Android",
                    buildClass.GetStatic<string>("MANUFACTURER"),
                    buildClass.GetStatic<string>("MODEL"),
                    versionClass.GetStatic<int>("SDK_INT"),
                    currentActivity.Call<string>("getPackageName"),
                    activityClass.Call<string>("getName"),
                    string.Empty);
            }
        }
        catch (Exception exception)
        {
            return AndroidPracticeSnapshot.CreateFailedSnapshot(exception.Message);
        }
#else
            return AndroidPracticeSnapshot.CreateEditorSnapshot();
#endif
        }
    }
}

public sealed class AndroidPracticeSnapshot
{
    #region 1. Fields

    //

    #endregion

    #region 2. Properties

    public string PlatformName { get; }
    public string Manufacturer { get; }
    public string Model { get; }
    public int SdkInt { get; }
    public string PackageName { get; }
    public string ActivityName { get; }
    public string ErrorMessage { get; }
    public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);

    #endregion

    #region 3. Constructor

    public AndroidPracticeSnapshot(
        string platformName,
        string manufacturer,
        string model,
        int sdkInt,
        string packageName,
        string activityName,
        string errorMessage)
    {
        PlatformName = platformName;
        Manufacturer = manufacturer;
        Model = model;
        SdkInt = sdkInt;
        PackageName = packageName;
        ActivityName = activityName;
        ErrorMessage = errorMessage;
    }

    #endregion

    #region 4. EventHandlers

    //


    public static AndroidPracticeSnapshot CreateEditorSnapshot()
    {
        return new AndroidPracticeSnapshot(
            Application.platform.ToString(),
            "Editor",
            SystemInfo.deviceModel,
            0,
            Application.identifier,
            "UnityEditor",
            string.Empty);
    }

    public static AndroidPracticeSnapshot CreateFailedSnapshot(string errorMessage)
    {
        return new AndroidPracticeSnapshot(
            "Android",
            string.Empty,
            string.Empty,
            0,
            string.Empty,
            string.Empty,
            errorMessage);
    }


    public override string ToString()
    {
        if (IsSuccess)
        {
            return
                $"AndroidPracticeSnapshot | Platform : {PlatformName}, Manufacturer : {Manufacturer}, Model : {Model}, SDK : {SdkInt}, Package : {PackageName}, Activity : {ActivityName}";
        }

        return $"AndroidPracticeSnapshot Failed | Error : {ErrorMessage}";
    }

    #endregion
}
