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
    private AudioClip _latestAlarmMusic;
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

        if (_alarmPopup == null || _soundData == null || _soundManager == null)
        {
            throw new NullReferenceException("AlarmPresenter's view or model is null");
        }

        SetDefaultState();
        BindEvent();
    }

    #endregion

    #region 4. Methods

    private void SetDefaultState()
    {
        _latestAlarmMusic = _soundData.AlarmSound;

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
        if (buttonType == EButtons.MusicOne)
        {
            _latestAlarmMusic = _soundData.AlarmSound;
        }
        else if (buttonType == EButtons.MusicTwo)
        {
            _latestAlarmMusic = _soundData.AlarmSound;
        }
        else if (buttonType == EButtons.MusicThree)
        {
            _latestAlarmMusic = _soundData.AlarmSound;
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
        _soundManager.CommandPlayingMusic(_latestAlarmMusic, 30f);
    }

    // todo 
    // UI 클릭 표시 갱신
    private void UpdateButtonsView()
    {
    }

    #endregion
}