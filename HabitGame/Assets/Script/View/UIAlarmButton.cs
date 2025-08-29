using System;
using UniRx;
using UnityEngine;

public class UIAlarmButton : UIButtonBase
{
    #region 1. Fields

    //test
    [SerializeField] private bool _isAlarmAudioClipButton;
    
    
    [SerializeField] private EAlarmButtonType _eAlarmButtonType;

    private readonly Subject<EAlarmButtonType> _onAlarmAudioClipButtonClicked = new();
    private readonly Subject<EAlarmButtonType> _onTimeButtonClicked = new();
    #endregion

    #region 2. Properties

    public IObservable<EAlarmButtonType> OnAlarmAudioClipButtonClicked => _onAlarmAudioClipButtonClicked;
    public IObservable<EAlarmButtonType> OnTimeButtonClicked => _onTimeButtonClicked;

    #endregion

    #region 3. Constructor

    protected override void OnAwake()
    {
        base.OnAwake();
    }

    

    #endregion

    #region 4. EventHandlers

    protected override void OnClickButton()
    {
        if (_isAlarmAudioClipButton)
        {
            _onAlarmAudioClipButtonClicked.OnNext(_eAlarmButtonType);
        }
        else
        {
            _onTimeButtonClicked.OnNext(_eAlarmButtonType);
        }
    }

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    // 

    #endregion
}