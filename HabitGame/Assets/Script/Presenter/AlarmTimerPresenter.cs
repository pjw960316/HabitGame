using System;
using UniRx;
using UnityEngine;

public class AlarmTimerPresenter : PresenterBase
{
    #region 1. Fields

    private UIAlarmTimerPopup _alarmTimerPopup;
    private AlarmData _alarmData;

    // 제거 요망?
    private AudioClip _alarmLoudAudioClip;
    private AudioClip _latestSleepingAudioClip;
    private float _latestAlarmPlayingTime;
    private TimeSpan _elapsedTime;

    private readonly CompositeDisposable _alarmDisposable = new();

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    public sealed override void Initialize(IView view)
    {
        base.Initialize(view);
        
        _alarmTimerPopup = _view as UIAlarmTimerPopup;
        _alarmData = _modelManager.GetModel<AlarmData>();

        if (_alarmData == null || _alarmTimerPopup == null)
        {
            throw new NullReferenceException("Model or View is null");
        }

        _latestSleepingAudioClip = _alarmData.LatestSleepingAudioClip;
        _latestAlarmPlayingTime = _alarmData.LatestAlarmPlayingTime;
        _alarmLoudAudioClip = _alarmData.WakeUpAudioClip;

        SetView();

        BindEvent();
    }

    protected override void SetView()
    {
        var titleText = _stringManager.GetUIString(EStringKey.EAlarmTimerPopupTitle, _latestAlarmPlayingTime);
        _alarmTimerPopup.SetAlarmHeaderText(titleText);
        
        ResetElapsedTime();
        var elapsedTimeString = $"{_elapsedTime.Hours:D2}:{_elapsedTime.Minutes:D2}:{_elapsedTime.Seconds:D2}";
        _alarmTimerPopup.UpdateAlarmTimerText(elapsedTimeString);
    }

    protected override void BindEvent()
    {
        // note : 여기서 OnClose()
        base.BindEvent();

        _alarmTimerPopup.OnQuitAlarm.Subscribe(_ => StopAlarmSystem()).AddTo(_disposable);

        Observable.Interval(TimeSpan.FromSeconds(1))
            .Subscribe(_ => RequestUpdateAlarmTimerPopupTime())
            .AddTo(_alarmDisposable);
    }

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    private void RequestUpdateAlarmTimerPopupTime()
    {
        _elapsedTime += TimeSpan.FromSeconds(1);
        var elapsedTimeString = $"{_elapsedTime.Hours:D2}:{_elapsedTime.Minutes:D2}:{_elapsedTime.Seconds:D2}";

        _alarmTimerPopup.UpdateAlarmTimerText(elapsedTimeString);
    }

    #endregion

    #region 6. Methods

    protected override void OnClosePopup()
    {
        // log
        Debug.Log("alarmTimerPresenter Terminate Presenter");

        TerminatePresenter();
    }

    private void ResetElapsedTime()
    {
        _elapsedTime = TimeSpan.Zero;
    }

    private void StopAlarmSystem()
    {
        _soundManager.RequestStopPlayMusic();

        DisposeAlarmSubscribe();

        _alarmTimerPopup.ClosePopup();
    }

    private void DisposeAlarmSubscribe()
    {
        _alarmDisposable?.Dispose();
    }

    #endregion
}