using UnityEngine;

public sealed class AndroidPracticeManager : ManagerBase<AndroidPracticeManager>
{
    #region 1. Fields

    private readonly AndroidPracticeBridge _androidPracticeBridge = new();

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    public override void Initialize()
    {
        TestAndroid();
    }

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    public AndroidPracticeSnapshot GetAndroidPracticeSnapshot()
    {
        return _androidPracticeBridge.GetSnapshot();
    }

    #endregion

    #region 6. Methods

    private void TestAndroid()
    {
        var snapshot = GetAndroidPracticeSnapshot();
        
        // 로그 찍어보자.
        Debug.Log(snapshot);
    }

    #endregion

    private sealed class AndroidPracticeBridge
    {
        public AndroidPracticeSnapshot GetSnapshot()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                using var buildClass = new AndroidJavaClass("android.os.Build");
                using var versionClass = new AndroidJavaClass("android.os.Build$VERSION");
                using var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                using var currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
                using var activityClass = currentActivity.Call<AndroidJavaObject>("getClass");

                return new AndroidPracticeSnapshot(
                    "Android",
                    buildClass.GetStatic<string>("MANUFACTURER"),
                    buildClass.GetStatic<string>("MODEL"),
                    versionClass.GetStatic<int>("SDK_INT"),
                    currentActivity.Call<string>("getPackageName"),
                    activityClass.Call<string>("getName"),
                    string.Empty);
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

    #endregion

    #region 5. Request Methods

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

    #endregion

    #region 6. Methods

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