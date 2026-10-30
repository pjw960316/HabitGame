using System;
using Unity.Notifications.Android;
using UnityEngine;

// NOTE : 책임
// 1. 모든 로컬 푸쉬를 관리한다. 
// 2. 
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

    // NOTE : 물 알림의 Java 호출 정보
    private const string WATER_NOTIFICATION_CLASS_NAME = "com.habitgame.notificationpractice.WaterNotificationReceiver";
    private const string ADD_CONFIRM_BUTTON_METHOD_NAME = "addConfirmButton";

    private AndroidNotification _waterNotification;
    private AndroidManager _androidManager;

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    public sealed override void Initialize()
    {
        _androidManager = AndroidManager.Instance;
        
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
    
    // TODO
    // 지금은 물 마시기 노티 하나만 관리하는 데...
    private void ScheduleNotifications()
    {
        ConvertWaterNotification();
    }

    private void ConvertWaterNotification()
    {
        var context = _androidManager.GetCurrentActivity();
        try
        {
            using (var builder = AndroidNotificationCenter.CreateNotificationBuilder(
                       WATER_NOTIFICATION_ID, _waterNotification, CHANNEL_ID))
            {
                object[] args = { context, builder, WATER_NOTIFICATION_ID };
                _androidManager.CallJavaMethod(WATER_NOTIFICATION_CLASS_NAME, ADD_CONFIRM_BUTTON_METHOD_NAME, args);

                AndroidNotificationCenter.SendNotification(builder);
            }
        }
        finally
        {
            // NOTE
            // 별거 아니지만, Dispose를 한 번 보고 오기 위해 using 말고.
            context?.Dispose();
        }
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
