using System;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

public class AlarmPresenter : PresenterBase
{
    #region 1. Fields

    // Note : View & Model
    private UIAlarmPopup _alarmPopup;
    private UIAlarmTimerPopup _alarmTimerPopup;
    private AlarmData _alarmData;

    private AudioClip _alarmLoudAudioClip;
    private AudioClip _latestSleepingAudioClip;
    private float _latestAlarmPlayingTime;

    private TimeSpan _elapsedTime;
    private readonly CompositeDisposable _alarmDisposable = new();
    // time

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    public sealed override void Initialize(IView view)
    {
        base.Initialize(view);

        _alarmPopup = _view as UIAlarmPopup;
        _alarmData = _dataManager.GetAlarmModel() as AlarmData;

        ExceptionHelper.CheckNullException(_alarmPopup, "_alarmPopup");
        ExceptionHelper.CheckNullException(_alarmData, "_alarmData");

        // ReSharper disable once PossibleNullReferenceException
        _latestSleepingAudioClip = _alarmData.GetDefaultAlarmAudioClip();
        _latestAlarmPlayingTime = _alarmData.GetDefaultAlarmTime();
        _alarmLoudAudioClip = _alarmData.AlarmChickenAudioClip;

        SetView();

        BindEvent();
    }

    protected override void SetView()
    {
        _alarmPopup.SetButtonText(_alarmData.AlarmTimeDictionary);
    }

    private void BindEvent()
    {
        foreach (var alarmAudioClipButton in _alarmPopup.AlarmAudioClipButtons)
        {
            alarmAudioClipButton.OnButtonClicked.Subscribe(UpdateAlarmAudioClip).AddTo(_disposable);
        }

        foreach (var alarmTimeButton in _alarmPopup.AlarmTimeButtons)
        {
            alarmTimeButton.OnButtonClicked.Subscribe(UpdateLatestTime).AddTo(_disposable);
        }

        _alarmPopup.OnConfirmed.Subscribe(_ => StartAlarmSystem()).AddTo(_disposable);
    }

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    // todo 
    // Presenter Connect를 관리하는 PresenterManager에서 connect 하도록
    private void RequestOpenAlarmTimerPopup()
    {
        var popupTargetTransform = _uiManager.MainCanvasTransform;

        _uiManager.OpenPopupByStringKey(EPopupKey.AlarmTimerPopup, popupTargetTransform);
        _alarmTimerPopup = _uiManager.GetPopupByStringKey<UIAlarmTimerPopup>(EPopupKey.AlarmTimerPopup);

        // note
        // alarmTimerSetting
        InitializeAlarmTimerPopupTime();
    }

    private void RequestPlaySleepingMusic(float sleepingMusicPlayingTime)
    {
        Observable.Interval(TimeSpan.FromSeconds(1))
            .Subscribe(_ => RequestUpdateAlarmTimerPopupTime())
            .AddTo(_alarmDisposable);

        /*
         * test code
         */
        /*Observable.Timer(TimeSpan.FromSeconds(sleepingMusicPlayingTime))
            .Subscribe(_ => RequestPlayLoudAlarmSound())
            .AddTo(_alarmDisposable);*/


        Observable.Timer(TimeSpan.FromMinutes(sleepingMusicPlayingTime))
            .Subscribe(_ => RequestPlayLoudAlarmSound())
            .AddTo(_alarmDisposable);

        _soundManager.SetAudioSourceLoopOn();
        _soundManager.RequestPlaySleepingMusic(_latestSleepingAudioClip);
    }

    private void InitializeAlarmTimerPopupTime()
    {
        ResetElapsedTime();
        var elapsedTimeString = $"{_elapsedTime.Hours:D2}:{_elapsedTime.Minutes:D2}:{_elapsedTime.Seconds:D2}";
        _alarmTimerPopup.UpdateAlarmTimerText(elapsedTimeString);
    }

    private void RequestUpdateAlarmTimerPopupTime()
    {
        _elapsedTime += TimeSpan.FromSeconds(1);
        var elapsedTimeString = $"{_elapsedTime.Hours:D2}:{_elapsedTime.Minutes:D2}:{_elapsedTime.Seconds:D2}";
        _alarmTimerPopup.UpdateAlarmTimerText(elapsedTimeString);
    }

    private void RequestPlayLoudAlarmSound()
    {
        ResetElapsedTime();
        DisposeAlarmSubscribe();

        _soundManager.SetAudioSourceLoopOn();
        _soundManager.RequestPlayLoudAlarmMusic(_alarmLoudAudioClip);
    }

    #endregion

    #region 6. Methods

    private void UpdateAlarmAudioClip(EAlarmButtonType eAlarmAudioClip)
    {
        _latestSleepingAudioClip = _alarmData.GetAlarmAudioClip(eAlarmAudioClip);
    }

    private void UpdateLatestTime(EAlarmButtonType eAlarmTime)
    {
        _latestAlarmPlayingTime = _alarmData.GetAlarmTime(eAlarmTime);
    }

    private void StartAlarmSystem()
    {
        RequestPlaySleepingMusic(_latestAlarmPlayingTime);

        CloseAlarmPopup();

        RequestOpenAlarmTimerPopup();
    }

    // refactor
    // Destroy를 쓰는 게 맞는가?
    private void CloseAlarmPopup()
    {
        Object.Destroy(_alarmPopup.gameObject);
    }


    private void ResetElapsedTime()
    {
        _elapsedTime = TimeSpan.Zero;
    }

    private void DisposeAlarmSubscribe()
    {
        _alarmDisposable?.Dispose();
    }

    #endregion
}