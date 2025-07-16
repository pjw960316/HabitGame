using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

using EButtons = AlarmPresenter.EButtons;

[RequireComponent(typeof(Button))]
public class UIAlarmPopup : UIPopupBase
{
    #region 1. Fields
    
    [Serializable]
    public struct ButtonData
    {
        public Button button;
        public EButtons buttonType;
    }
    [SerializeField] private List<ButtonData> _alarmMusicButtons = new();
    [SerializeField] private List<ButtonData> _timeButtons = new();
    [SerializeField] private Button _confirmButton;

    private UIManager _uiManager;
    private SoundManager _soundManager;
    private AlarmPresenter _alarmPresenter;

    private readonly Subject<EButtons> _onAlarmMusicButtonClicked = new();
    public IObservable<EButtons> OnAlarmMusicButtonClicked => _onAlarmMusicButtonClicked;

    private readonly Subject<EButtons> _onTimeButtonClicked = new();
    public IObservable<EButtons> OnTimeButtonClicked => _onTimeButtonClicked;

    private readonly Subject<Unit> _onConfirmed = new();
    public IObservable<Unit> OnConfirmed => _onConfirmed;

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    protected void Awake()
    {
        _uiManager = UIManager.Instance;
        _soundManager = SoundManager.Instance;
        _alarmPresenter = _soundManager.GetPresenterAfterCreate<AlarmPresenter>(this);

        if (_alarmPresenter == null)
        {
            throw new NullReferenceException("_alarmPresenter");
        }

        BindEvent();
    }

    #endregion

    #region 4. Methods

    private void BindEvent()
    {
        BindButtonsEvent(_alarmMusicButtons, _onAlarmMusicButtonClicked);
        BindButtonsEvent(_timeButtons, _onTimeButtonClicked);
        
        _confirmButton?.onClick.AddListener(() => _onConfirmed.OnNext(Unit.Default));
    }

    private void BindButtonsEvent(List<ButtonData> buttonDataList, Subject<EButtons> subject)
    {
        foreach (var buttonData in buttonDataList)
        {
            buttonData.button.onClick.AddListener(() => { subject.OnNext(buttonData.buttonType); });
        }
    }

    #endregion

    #region 5. EventHandlers

    //default

    #endregion
}