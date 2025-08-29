using System;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

public class AlarmPresenter : PresenterBase
{
    #region 1. Fields

    // Note : View & Model
    private UIAlarmPopup _alarmPopup;
    private AlarmData _alarmData;

    // Note : Manager
    private SoundManager _soundManager;
    private UIToastManager _uiToastManager;

    private AudioClip _latestSleepingAudioClip;
    private float _latestAlarmPlayingTime;
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

        // todo: 알람데이터 세팅

        _soundManager = SoundManager.Instance;
        _uiToastManager = UIToastManager.Instance;

        ExceptionHelper.CheckNullException(_alarmPopup, "_alarmPopup");
        ExceptionHelper.CheckNullException(_soundManager, "_soundManager");

        _latestSleepingAudioClip = _alarmData.GetDefaultAlarmAudioClip();
        _latestAlarmPlayingTime = _alarmData.GetDefaultAlarmTime();

        SetView();

        BindEvent();
    }

    protected override void SetView()
    {
    }

    private void BindEvent()
    {
        _alarmPopup.OnAlarmAudioClipButtonClicked.Subscribe(UpdateAlarmAudioClip).AddTo(_disposable);
        _alarmPopup.OnTimeButtonClicked.Subscribe(UpdateLatestTime).AddTo(_disposable);
        _alarmPopup.OnConfirmed.Subscribe(_ => StartAlarm()).AddTo(_disposable);
    }

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    // 

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

    private void StartAlarm()
    {
        RequestStartingAlarm(_latestAlarmPlayingTime);
        
        CloseAlarmPopup();

        _uiToastManager.ShowToast(EToastStringKey.EAlarmConfirm);
    }

    private void RequestStartingAlarm(float playingTime)
    {
        Observable.Timer(TimeSpan.FromSeconds(playingTime)).Subscribe(_ => RequestPlayingWakeUpSound())
            .AddTo(_disposable);

        _soundManager.SetAudioSourceLoop();
        _soundManager.CommandPlayingMusic(_latestSleepingAudioClip);
    }

    private void RequestPlayingWakeUpSound()
    {
        _soundManager.CommandPlayingWakeUpSound();
    }

    // refactor
    // Destroy를 쓰는 게 맞는가?
    private void CloseAlarmPopup()
    {
        Object.Destroy(_alarmPopup.gameObject);
    }

    #endregion
}