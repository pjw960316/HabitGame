using System;
using UniRx;
using UnityEngine;

public class AlarmTimerPresenter : UIPresenterBase
{
    #region 1. Fields

    private UIAlarmTimerPopup _alarmTimerPopup;
    private AlarmData _alarmData;

    private AudioClip _alarmLoudAudioClip;
    private float _latestSleepingAudioPlayTime;

    private TimeSpan _elapsedTime;

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    public sealed override void Initialize(IView view)
    {
        base.Initialize(view);

        _latestSleepingAudioPlayTime = _alarmData.LatestSleepingAudioPlayTime;
        _alarmLoudAudioClip = _alarmData.AlarmAudioClip;
    }

    protected sealed override void InitializeView()
    {
        base.InitializeView();

        _alarmTimerPopup = _view as UIAlarmTimerPopup;
        ExceptionHelper.CheckNullException(_alarmTimerPopup, "_alarmTimerPopup");
    }

    protected sealed override void InitializeModel()
    {
        _alarmData = _modelManager.GetModel<AlarmData>();
        ExceptionHelper.CheckNullException(_alarmData, "_alarmData");
    }

    public sealed override void SetView()
    {
        var titleText = _stringManager.GetUIString(EStringKey.EAlarmTimerPopupTitle, _latestSleepingAudioPlayTime);
        _alarmTimerPopup.SetAlarmHeaderText(titleText);

        _elapsedTime = TimeSpan.Zero;
        var elapsedTimeString = $"{_elapsedTime.Hours:D2}:{_elapsedTime.Minutes:D2}:{_elapsedTime.Seconds:D2}";
        _alarmTimerPopup.UpdateAlarmTimerText(elapsedTimeString);
    }

    public sealed override void BindEvent()
    {
        _alarmTimerPopup.OnQuitAlarm.Subscribe(_ => OnStopAlarmSystem()).AddTo(_disposable);

        Observable.Timer(TimeSpan.FromMinutes(_latestSleepingAudioPlayTime))
            .Subscribe(_ => RequestPlayAlarm())
            .AddTo(_disposable);

        Observable.Interval(TimeSpan.FromSeconds(1))
            .Subscribe(_ => RequestUpdateAlarmTimerPopupTime())
            .AddTo(_disposable);
    }

    #endregion

    #region 4. EventHandlers

    private void OnStopAlarmSystem()
    {
        _soundManager.StopPlayMusic();
        _soundManager.PlayBackgroundMusic();

        _alarmData.RestoreAudioClipAndPlayTimeToDefault();

        Close();
    }

    #endregion

    #region 5. Request Methods

    private void RequestUpdateAlarmTimerPopupTime()
    {
        _elapsedTime += TimeSpan.FromSeconds(1);
        var elapsedTimeString = $"{_elapsedTime.Hours:D2}:{_elapsedTime.Minutes:D2}:{_elapsedTime.Seconds:D2}";

        _alarmTimerPopup.UpdateAlarmTimerText(elapsedTimeString);
    }

    private void RequestPlayAlarm()
    {
        _soundManager.SetAudioSourceLoopOn();
        _soundManager.PlayMusic(_alarmLoudAudioClip);
    }

    #endregion

    #region 6. Methods

    //

    #endregion
}