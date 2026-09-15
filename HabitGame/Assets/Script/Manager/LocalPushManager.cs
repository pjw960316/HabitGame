using System;
using Unity.Notifications.Android;

public sealed class LocalPushManager : ManagerBase<LocalPushManager>
{
    #region 1. Fields

    // NOTE : Channel
    // 채널은 현재 하나만 두고 있다.
    private const string CHANNEL_ID = "HabitNotificationChannel";
    private const string CHANNEL_NAME = "Habit_Notification";
    private const string CHANNEL_DESCRIPTION = "Habit_Notification";

    // NOTE : Channel 내부의 push 알림들
    private const int WATER_NOTIFICATION_ID = 1001;
    private const int WATER_NOTIFICATION_HOUR = 21;
    private const string WATER_NOTIFICATION_TITLE = "물 마시자";
    private const string WATER_NOTIFICATION_TEXT = "물 500mL 마시자 \n그래야 야식 생각이 나지 않는다.";
    
    private AndroidNotification _waterNotification;

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    public sealed override void Initialize()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        InitializeNotificationChannel();
        InitializeNotifications();

        ScheduleNotifications();
#endif
    }

    private void InitializeNotificationChannel()
    {
        var channel = new AndroidNotificationChannel(CHANNEL_ID, CHANNEL_NAME, CHANNEL_DESCRIPTION, Importance.Low);

        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    private void InitializeNotifications()
    {
        InitializeDrinkWaterNotification();
    }
    
    private void InitializeDrinkWaterNotification()
    {
        _waterNotification = new AndroidNotification(WATER_NOTIFICATION_TITLE, WATER_NOTIFICATION_TEXT,
            GetWaterNotificationTime(), TimeSpan.FromDays(1));
    }
    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Methods
    
    private void ScheduleNotifications()
    {
        AndroidNotificationCenter.SendNotificationWithExplicitID(
            _waterNotification, CHANNEL_ID, WATER_NOTIFICATION_ID);
    }
    
    private DateTime GetWaterNotificationTime()
    {
        var now = DateTime.Now;
        var todayWaterNotificationTime = now.Date.AddHours(WATER_NOTIFICATION_HOUR);

        if (todayWaterNotificationTime <= now)
        {
            return todayWaterNotificationTime.AddDays(1);
        }

        return todayWaterNotificationTime;
    }

    #endregion
}
