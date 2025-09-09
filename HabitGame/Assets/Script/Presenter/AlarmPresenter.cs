using System;
using UniRx;
using UnityEngine;

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

    // note
    // AlarmDisposable은 UIAlarmPopup이 종료되어도 동작해야 한다.
    // 그러므로 PresenterBase의 _disposable과 생명주기를 다르게 해야한다.
    private readonly CompositeDisposable _alarmDisposable = new();

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    public sealed override void Initialize(IView view)
    {
        base.Initialize(view);

        _alarmPopup = _view as UIAlarmPopup;
        _alarmData = _modelManager.GetModel<AlarmData>();

        ExceptionHelper.CheckNullException(_alarmPopup, "_alarmPopup");
        if (_alarmData == null)
        {
            throw new NullReferenceException("_alarmData");
        }

        _latestSleepingAudioClip = _alarmData.GetDefaultAlarmAudioClip();
        _latestAlarmPlayingTime = _alarmData.GetDefaultAlarmTime();
        _alarmLoudAudioClip = _alarmData.AlarmChickenAudioClip;

        SetView();

        BindEvent();
    }

    protected sealed override void SetView()
    {
        _alarmPopup.SetButtonText(_alarmData.AlarmTimeDictionary);
    }

    protected sealed override void BindEvent()
    {
        base.BindEvent();

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

    protected override void OnClosePopup()
    {
        Debug.Log("call");
        if (shouldTerminatePresenter())
        {
            // log
            Debug.Log("AlarmPresenter 의 Presenter 제거");
            
            TerminatePresenter();
        }
    }

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
        InitializeAlarmTimerPopup();
    }

    private void RequestPlaySleepingMusic(float sleepingMusicPlayingTime)
    {
        Observable.Interval(TimeSpan.FromSeconds(1))
            .Subscribe(_ => RequestUpdateAlarmTimerPopupTime())
            .AddTo(_alarmDisposable);

        Observable.Timer(TimeSpan.FromMinutes(sleepingMusicPlayingTime))
            .Subscribe(_ => RequestPlayLoudAlarmSound())
            .AddTo(_alarmDisposable);

        _soundManager.RequestAudioSourceLoopOn();
        _soundManager.RequestPlaySleepingMusic(_latestSleepingAudioClip);
    }

    private void InitializeAlarmTimerPopup()
    {
        var titleText = _stringManager.GetUIString(EStringKey.EAlarmTimerPopupTitle, _latestAlarmPlayingTime);
        _alarmTimerPopup.SetAlarmHeaderText(titleText);

        ResetElapsedTime();

        var elapsedTimeString = $"{_elapsedTime.Hours:D2}:{_elapsedTime.Minutes:D2}:{_elapsedTime.Seconds:D2}";

        _alarmTimerPopup.UpdateAlarmTimerText(elapsedTimeString);
        _alarmTimerPopup.OnQuitAlarm.Subscribe(_ => StopAlarmSystem()).AddTo(_disposable);
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

        _soundManager.RequestAudioSourceLoopOn();
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
        _uiManager.AddPendingPopup(EPopupKey.AlarmTimerPopup);

        _alarmPopup.ClosePopup();

        RequestPlaySleepingMusic(_latestAlarmPlayingTime);

        RequestOpenAlarmTimerPopup();
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

    private bool shouldTerminatePresenter()
    {
        if (_uiManager.IsPopupOpeningOrOpened(EPopupKey.AlarmPopup) ||
            _uiManager.IsPopupOpeningOrOpened(EPopupKey.AlarmTimerPopup))
        {
            return false;
        }

        return true;
    }

    #endregion
}