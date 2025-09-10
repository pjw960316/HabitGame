using System;
using UniRx;
using UnityEngine;

public class AlarmPresenter : PresenterBase
{
    #region 1. Fields

    private UIAlarmPopup _alarmPopup;
    private AlarmData _alarmData;

    private AudioClip _alarmLoudAudioClip;
    private AudioClip _latestSleepingAudioClip;
    private float _latestAlarmPlayingTime;

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
        // log
        Debug.Log("alarmPresenter Terminate Presenter");

        TerminatePresenter();
    }

    #endregion

    #region 5. Request Methods

    // todo 
    // Presenter Connect를 관리하는 PresenterManager에서 connect 하도록
    private void RequestOpenAlarmTimerPopup()
    {
        var popupTargetTransform = _uiManager.MainCanvasTransform;

        _uiManager.OpenPopupByStringKey(EPopupKey.AlarmTimerPopup, popupTargetTransform);
        _uiManager.GetPopupByStringKey<UIAlarmTimerPopup>(EPopupKey.AlarmTimerPopup);
    }

    private void RequestPlaySleepingMusic(float sleepingMusicPlayingTime)
    {
        //refactor 이거 관리
        Observable.Timer(TimeSpan.FromMinutes(sleepingMusicPlayingTime))
            .Subscribe(_ => RequestPlayLoudAlarmSound())
            .AddTo(_alarmDisposable);

        _soundManager.RequestAudioSourceLoopOn();
        _soundManager.RequestPlaySleepingMusic(_latestSleepingAudioClip);
    }

    private void RequestPlayLoudAlarmSound()
    {
        // refactor
        // 얘도 옮겨야 할 듯?
        //ResetElapsedTime();
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

    private void DisposeAlarmSubscribe()
    {
        _alarmDisposable?.Dispose();
    }

    #endregion
}