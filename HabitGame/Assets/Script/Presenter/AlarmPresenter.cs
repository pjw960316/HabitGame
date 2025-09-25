using System;
using UniRx;
using UnityEngine;

public class AlarmPresenter : UIPresenterBase
{
    #region 1. Fields

    private UIAlarmPopup _alarmPopup;
    private AlarmData _alarmData;

    #endregion

    #region 2. Properties

    //

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

        SetView();
        
        BindEvent();
    }

    protected sealed override void SetView()
    {
        _alarmPopup.SetButtonText(_alarmData.SleepingAudioPlayTimeDictionary);
    }

    protected sealed override void BindEvent()
    {
        base.BindEvent();

        foreach (var alarmAudioClipButton in _alarmPopup.AlarmAudioClipButtons)
        {
            alarmAudioClipButton.OnButtonClicked.Subscribe(RequestUpdateLatestSleepingAudioClip).AddTo(_disposable);
        }

        foreach (var alarmTimeButton in _alarmPopup.AlarmTimeButtons)
        {
            alarmTimeButton.OnButtonClicked.Subscribe(RequestUpdateLatestSleepingAudioPlayTime).AddTo(_disposable);
        }

        _alarmPopup.OnConfirmed.Subscribe(_ => OnStartAlarmSystem()).AddTo(_disposable);
    }

    #endregion

    #region 4. EventHandlers

    protected override void OnClosePopup()
    {
        // log
        Debug.Log("alarmPresenter Terminate Presenter");

        TerminatePresenter();
    }

    private void OnStartAlarmSystem()
    {
        _uiManager.AddPendingPopup(EPopupKey.AlarmTimerPopup);

        _alarmPopup.ClosePopup();

        RequestPlaySleepingMusic();

        RequestOpenAlarmTimerPopup();
    }

    #endregion

    #region 5. Request Methods

    private void RequestOpenAlarmTimerPopup()
    {
        var popupTargetTransform = _uiManager.MainCanvasTransform;

        _uiManager.OpenPopupByStringKey(EPopupKey.AlarmTimerPopup, popupTargetTransform);
    }

    private void RequestPlaySleepingMusic()
    {
        _soundManager.RequestAudioSourceLoopOn();
        _soundManager.RequestPlaySleepingMusic(_alarmData.LatestSleepingAudioClip);
    }

    private void RequestUpdateLatestSleepingAudioClip(EAlarmButtonType eAlarmAudioClip)
    {
        _alarmData.SetLatestSleepingAudioClip(eAlarmAudioClip);
    }

    private void RequestUpdateLatestSleepingAudioPlayTime(EAlarmButtonType eAlarmTime)
    {
        _alarmData.SetLatestSleepingAudioPlayTime(eAlarmTime);
    }

    #endregion

    #region 6. Methods

    //

    #endregion
}