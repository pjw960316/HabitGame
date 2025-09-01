using System;
using UniRx;
using UnityEngine;

// note
// AlarmButton = AudioClipButton + TimeButton
public class UIAlarmButton : UIButtonBase
{
    #region 1. Fields

    [SerializeField] private EAlarmButtonType _eAlarmButtonType;
    [SerializeField] private Color _clickedColor;
    [SerializeField] private Color _notClickedColor;

    private readonly Subject<EAlarmButtonType> _onButtonClicked = new();

    #endregion

    #region 2. Properties

    public EAlarmButtonType AlarmButtonType => _eAlarmButtonType;
    public IObservable<EAlarmButtonType> OnButtonClicked => _onButtonClicked;
    public bool IsSelected { get; private set; }

    #endregion

    #region 3. Constructor

    
    // note
    // Unity는 Awake의 호출 순서를 보장하지 않는다.
    // Popup의 Awake가 콜이 되어도 Widget의 콜은 늦을 수 있다.
    // 그러므로 Initialize()로 관리
    protected override void OnAwake()
    {
        base.OnAwake();
    }

    public sealed override void Initialize()
    {
        base.Initialize();
        
        _isAutoText = false;
        IsSelected = false;
        
        UpdateButtonColor(_notClickedColor);
    }

    #endregion

    #region 4. EventHandlers

    protected override void OnClickButton()
    {
        _onButtonClicked.OnNext(_eAlarmButtonType);
    }

    #endregion

    #region 5. Request Methods

    //

    #endregion

    #region 6. Methods

    public void UpdateAlarmButton(bool isSelected)
    {
        IsSelected = isSelected;
        UpdateButtonColor(IsSelected ? _clickedColor : _notClickedColor);
    }

    public void UpdateAlarmButtonText(float time)
    {
        _buttonText.text = _stringManager.GetUIString(EStringKey.EAlarmPopupAlarmTime, time);
    }

    #endregion
}