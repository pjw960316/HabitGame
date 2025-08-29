using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIAlarmPopup : UIPopupBase
{
    #region 1. Fields

    [SerializeField] private List<UIButtonBase> _alarmAudioClipButtons = new();
    [SerializeField] private List<UIButtonBase> _alarmTimeButtons = new();
    [SerializeField] private UIButtonBase _confirmButton;

    private AlarmPresenter _alarmPresenter;

    private readonly Subject<EAlarmButtonType> _onAlarmAudioClipButtonClicked = new();
    private readonly Subject<EAlarmButtonType> _onTimeButtonClicked = new();


    private readonly Subject<Unit> _onConfirmed = new();

    #endregion

    #region 2. Properties

    public IObservable<EAlarmButtonType> OnAlarmAudioClipButtonClicked => _onAlarmAudioClipButtonClicked;
    public IObservable<EAlarmButtonType> OnTimeButtonClicked => _onTimeButtonClicked;
    public IObservable<Unit> OnConfirmed => _onConfirmed;

    #endregion

    #region 3. Constructor

    public override void OnAwake()
    {
        base.OnAwake();

        Initialize();

        CreatePresenterByManager();

        BindEvent();
    }

    private void Initialize()
    {
    }

    protected sealed override void CreatePresenterByManager()
    {
        _uiManager.CreatePresenter<AlarmPresenter>(this);
    }

    #endregion

    private void BindEvent()
    {
        //todo : Widget에 바인딩 하자
        foreach (var alarmWidget in _alarmAudioClipButtons)
        {
        }

        foreach (var timeWidget in _alarmTimeButtons)
        {
        }

        _confirmButton?.OnClick.AddListener(() => _onConfirmed.OnNext(Unit.Default));
    }


    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    /*private void CommandUpdateButtonColor(UIButtonBase clickedButtonData)
    {
        var clickedButtonType = clickedButtonData.buttonType;

        switch (clickedButtonType)
        {
            case EButtons.DivisionConst:
                throw new InvalidDataException("buttonType은 DivisionConst가 될 수 없다.");
            case < EButtons.DivisionConst:
                InternalUpdateButtonColor(clickedButtonType, _alarmMusicButtons);
                break;
            case > EButtons.DivisionConst:
                InternalUpdateButtonColor(clickedButtonType, _timeButtons);
                return;
        }
    }

    private void InternalUpdateButtonColor(EButtons clickedButtonType, List<ButtonData> list)
    {
        foreach (var buttonData in list)
        {
            buttonData.button.image.color = _buttonColorDictionary[buttonData.buttonType == clickedButtonType];
        }
    }*/

    #endregion

    #region 6. Methods

    // 

    #endregion
}