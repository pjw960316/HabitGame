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

    private AudioClip _alarmLoudAudioClip;
    private AudioClip _latestSleepingAudioClip;
    private float _latestAlarmPlayingTime;

    private TimeSpan _elapsedSleepingAudioClipPlayingTime;
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

    private void RequestPlaySleepingMusic(float sleepingMusicPlayingTime)
    {
        Observable.Interval(TimeSpan.FromSeconds(1))
            .Subscribe(_ => Test())
            .AddTo(_alarmDisposable);
        
        Observable.Timer(TimeSpan.FromMinutes(sleepingMusicPlayingTime))
            .Subscribe(_ => RequestPlayLoudAlarmSound())
            .AddTo(_alarmDisposable);

        _soundManager.SetAudioSourceLoopOn();
        _soundManager.RequestPlaySleepingMusic(_latestSleepingAudioClip);
    }

    private void Test()
    {
        _elapsedSleepingAudioClipPlayingTime += TimeSpan.FromSeconds(1);
        var cachedTime = _elapsedSleepingAudioClipPlayingTime; //too long Name
        
        var elapsedTimeString = $"{cachedTime.Hours:D2}:{cachedTime.Minutes:D2}:{cachedTime.Seconds:D2}";
    }

    private void RequestPlayLoudAlarmSound()
    {
        Debug.Log("수면 음악 종료 알람 시작");
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

        OpenAlarmTimerPopup();

        _uiToastManager.ShowToast(EToastStringKey.EAlarmConfirm);
    }

    // refactor
    // Destroy를 쓰는 게 맞는가?
    private void CloseAlarmPopup()
    {
        Object.Destroy(_alarmPopup.gameObject);
    }

    private void OpenAlarmTimerPopup()
    {
        //_uiM
    }

    private void DisposeAlarmSubscribe()
    {
        _alarmDisposable?.Dispose();
    }

    #endregion
}