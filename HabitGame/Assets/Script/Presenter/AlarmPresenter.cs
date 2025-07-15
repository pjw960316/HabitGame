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
        TimeThree,
    }
    
    private UIAlarmPopup uiAlarmPopup;
    
    //test
    private AudioClip _testClip;

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    public sealed override void Initialize(IView view)
    {
        base.Initialize(view);

        uiAlarmPopup = view as UIAlarmPopup;

        if (uiAlarmPopup == null)
        {
            throw new NullReferenceException("UIAlarmPopup");
        }

        BindEvent();
    }

    #endregion

    #region 4. Methods

    private void BindEvent()
    {
        uiAlarmPopup.OnAlarmMusicButtonClicked.Subscribe(SetAfterRequestAlarmMusic).AddTo(Disposable);
        uiAlarmPopup.OnTimeButtonClicked.Subscribe(_ => GetTime()).AddTo(Disposable);
        uiAlarmPopup.OnConfirmed.Subscribe(_ => UpdateView()).AddTo(Disposable);
    }

    #endregion

    #region 5. EventHandlers

    //test
    //전체가 테스트 코드 with connect model
    private void SetAfterRequestAlarmMusic(EButtons buttonType)
    {
        var soundData = Model as SoundData;

        if (buttonType == EButtons.MusicOne)
        {
            _testClip = soundData.AlarmSound;
        }
    }

    private void GetTime()
    {
        
    }
    

    private void UpdateView()
    {
    }

    #endregion
}