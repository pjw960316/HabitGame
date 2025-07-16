using System;
using UniRx;
using UnityEngine;

public class AlarmPresenter : PresenterBase
{
    #region 1. Fields

    public enum EButtons
    {
        MusicOne,
        MusicTwo,
        MusicThree,

        TimeOne,
        TimeTwo,
        TimeThree
    }

    //NOTE
    //MODEL & VIEW & Manager
    private SoundData _soundData;
    private UIAlarmPopup _alarmPopup;
    private SoundManager _soundManager;

    //test
    private AudioClip _latestSleepingAudioClip;
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

        if (_alarmPopup == null)
        {
            throw new NullReferenceException("_alarmPopup is null");
        }
        if (_soundData == null)
        {
            throw new NullReferenceException("_soundData is null");
        }
        if (_soundManager == null)
        {
            throw new NullReferenceException("_soundManager is null");
        }

        SetDefaultState();
        BindEvent();
    }

    #endregion

    #region 4. Methods

    private void SetDefaultState()
    {
        _latestSleepingAudioClip = _soundData.FirstSleepingAudioClip;

        //시간
    }

    private void BindEvent()
    {
        _alarmPopup.OnAlarmMusicButtonClicked.Subscribe(SetLatestAlarmMusic).AddTo(Disposable);
        _alarmPopup.OnTimeButtonClicked.Subscribe(SetLatestTime).AddTo(Disposable);
        _alarmPopup.OnConfirmed.Subscribe(_ => StartAlarm()).AddTo(Disposable);
    }

    #endregion

    #region 5. EventHandlers

    private void SetLatestAlarmMusic(EButtons buttonType)
    {
        //NOTE
        //이런 건 XML을 통해 Matching을 했었다.
        
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

    private void SetLatestTime(EButtons buttonType)
    {
        if (buttonType == EButtons.TimeOne)
        {
        }
    }


    // todo
    // 매니저에게 지금 정보 알려주고 재생 시키고 View 갱신
    private void StartAlarm()
    {
        RequestStartingAlarm();
        UpdateButtonsView();
    }

    private void RequestStartingAlarm()
    {
        //test 30초
        _soundManager.SetAudioSourceLoop();
        _soundManager.CommandPlayingMusic(_latestSleepingAudioClip, 30f);
    }

    // todo 
    // UI 클릭 표시 갱신
    private void UpdateButtonsView()
    {
    }

    #endregion
}