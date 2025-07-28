using System;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

public class AlarmPresenter : PresenterBase
{
    #region 1. Fields

    public enum EButtons
    {
        MusicOne,
        MusicTwo,
        MusicThree,

        DivisionConst,

        TimeOne,
        TimeTwo,
        TimeThree
    }

    //NOTE
    //MODEL & VIEW & Manager
    private SoundData _soundData;
    private UIAlarmPopup _alarmPopup;
    private SoundManager _soundManager;

    //refactor
    //모델이 두개?
    private ViewData _viewData;

    //test
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

        _alarmPopup = View as UIAlarmPopup;
        _soundData = Model as SoundData;
        _soundManager = SoundManager.Instance;

        ExceptionHelper.CheckNullException(_alarmPopup, "_alarmPopup");
        ExceptionHelper.CheckNullException(_soundData, "_soundData");
        ExceptionHelper.CheckNullException(_soundManager, "_soundManager");

        //refactor
        //모델이 2개일 때
        //걍 property로 해도 되나?
        _viewData = UIManager.Instance.ViewData;

        SetDefaultState();
        BindEvent();
    }

    #endregion

    #region 4. Methods

    private void SetDefaultState()
    {
        _latestSleepingAudioClip = _soundData.FirstSleepingAudioClip;
        _latestAlarmPlayingTime = _viewData.AlarmTimeDictionary[EButtons.TimeOne];
    }

    private void BindEvent()
    {
        _alarmPopup.OnAlarmMusicButtonClicked.Subscribe(UpdateLatestAlarmMusic).AddTo(Disposable);
        _alarmPopup.OnTimeButtonClicked.Subscribe(UpdateLatestTime).AddTo(Disposable);
        _alarmPopup.OnConfirmed.Subscribe(_ => StartAlarm()).AddTo(Disposable);
    }

    #endregion

    #region 5. EventHandlers

    // refactor
    private void UpdateLatestAlarmMusic(EButtons buttonType)
    {
        // test
        // 일단 다 first
        if (buttonType == EButtons.MusicOne)
        {
            _latestSleepingAudioClip = _soundData.FirstSleepingAudioClip;
        }
        else if (buttonType == EButtons.MusicTwo)
        {
            _latestSleepingAudioClip = _soundData.FirstSleepingAudioClip;
        }
        else if (buttonType == EButtons.MusicThree)
        {
            _latestSleepingAudioClip = _soundData.FirstSleepingAudioClip;
        }
    }

    private void UpdateLatestTime(EButtons buttonType)
    {
        _latestAlarmPlayingTime = _viewData.AlarmTimeDictionary[buttonType];
    }

    // todo
    // 매니저에게 지금 정보 알려주고 재생 시키고 View 갱신
    private void StartAlarm()
    {
        RequestStartingAlarm(_latestAlarmPlayingTime);
        CloseAlarmPopup();

        UIToastManager.Instance.ShowToastMessage();
    }

    private void RequestStartingAlarm(float playingTime)
    {
        Observable.Timer(TimeSpan.FromSeconds(playingTime)).Subscribe(_ => RequestPlayingWakeUpSound())
            .AddTo(Disposable);

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