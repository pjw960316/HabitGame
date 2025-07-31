using System;
using System.Collections.Generic;
using System.IO;
using AYellowpaper.SerializedCollections;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using EButtons = AlarmPresenter.EButtons;

[RequireComponent(typeof(Button))]
public class UIAlarmPopup : UIPopupBase
{
    #region 1. Fields

    //refactor
    //ButtonData 대신에 Widget으로.
    [Serializable]
    public class ButtonData
    {
        public Button button;
        public EButtons buttonType;
    }

    [SerializeField] private List<ButtonData> _alarmMusicButtons = new();
    [SerializeField] private List<ButtonData> _timeButtons = new();
    [SerializeField] private Button _confirmButton;

    [SerializeField] private SerializedDictionary<EStringKey, TextMeshProUGUI> _buttonTexts = new();
    [SerializeField] private SerializedDictionary<bool, Color> _buttonColorDictionary = new();

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
        _soundManager = SoundManager.Instance;
        _uiManager = UIManager.Instance;
        
        _uiManager.CreatePresenter<AlarmPresenter>(this);
        
        SetInitialUIState();
        BindEvent();
    }

    #endregion

    #region 4. Methods

    private void SetInitialUIState()
    {
        SetButtonTexts();
    }

    protected sealed override void BindEventInternal()
    {
        BindButtonsEvent(_alarmMusicButtons, _onAlarmMusicButtonClicked);
        BindButtonsEvent(_timeButtons, _onTimeButtonClicked);

        _confirmButton?.onClick.AddListener(() => _onConfirmed.OnNext(Unit.Default));
    }

    private void BindButtonsEvent(List<ButtonData> buttonDataList, Subject<EButtons> subject)
    {
        foreach (var buttonData in buttonDataList)
        {
            buttonData.button.onClick.AddListener(() => { OnClickButton(buttonData, subject); });
        }
    }

    private void SetButtonTexts()
    {
        var stringManager = StringManager.Instance;

        foreach (var buttonText in _buttonTexts)
        {
            buttonText.Value.text = stringManager.GetUIString(buttonText.Key);
        }
    }

    #endregion

    #region 5. EventHandlers

    //test
    private void OnClickButton(ButtonData buttonData, Subject<EButtons> subject)
    {
        subject.OnNext(buttonData.buttonType);
        UpdateButtonColor(buttonData);
    }

    //Note
    //View Method
    private void UpdateButtonColor(ButtonData clickedButtonData)
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
    }

    private void OnDestroy()
    {
        //default
    }

    #endregion
}