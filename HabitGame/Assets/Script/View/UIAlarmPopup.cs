using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIAlarmPopup : UIPopupBase
{
    #region 1. Fields

    [SerializeField] private List<UIAlarmButton> _alarmAudioClipButtons = new();
    [SerializeField] private List<UIAlarmButton> _alarmTimeButtons = new();
    [SerializeField] private UIButtonBase _confirmButton;

    private readonly Subject<Unit> _onConfirmed = new();

    #endregion

    #region 2. Properties

    public List<UIAlarmButton> AlarmAudioClipButtons => _alarmAudioClipButtons;
    public List<UIAlarmButton> AlarmTimeButtons => _alarmTimeButtons;
    public IObservable<Unit> OnConfirmed => _onConfirmed;

    #endregion

    #region 3. Constructor

    protected override void OnAwake()
    {
        base.OnAwake();
    }

    protected sealed override void Initialize()
    {
        base.Initialize();
        
        InitializeWidgets();
    }

    private void InitializeWidgets()
    {
        foreach (var widget in AlarmAudioClipButtons)
        {
            widget.Initialize();
        }

        foreach (var widget in AlarmTimeButtons)
        {
            widget.Initialize();
        }
    }

    protected sealed override void CreatePresenterByManager()
    {
        _uiManager.CreatePresenter<AlarmPresenter>(this);
    }

    #endregion

    protected sealed override void BindEvent()
    {
        BindButtonMenuEvents(AlarmAudioClipButtons);
        BindButtonMenuEvents(AlarmTimeButtons);

        _confirmButton?.OnClick.AddListener(() => _onConfirmed.OnNext(Unit.Default));
    }

    public void SetButtonText(ImmutableDictionary<EAlarmButtonType, float> immutableDictionary)
    {
        foreach (var widget in _alarmTimeButtons)
        {
            if (immutableDictionary.TryGetValue(widget.AlarmButtonType, out var time))
            {
                widget.UpdateAlarmButtonText(time);
            }
            else
            {
                Debug.Log($"{widget.AlarmButtonType} 의 알람 버튼의 텍스트가 세팅되지 않았습니다.");
            }
        }
    }

    #region 4. EventHandlers

    private void BindButtonMenuEvents(List<UIAlarmButton> list)
    {
        foreach (var widget in list)
        {
            widget.OnButtonClicked.Subscribe(_ => { RequestUpdateButtonColor(widget.AlarmButtonType); })
                .AddTo(_disposables);
        }
    }

    #endregion

    #region 5. Request Methods

    private void RequestUpdateButtonColor(EAlarmButtonType eAlarmButtonType)
    {
        switch (eAlarmButtonType)
        {
            case EAlarmButtonType.DivisionConst:
                throw new InvalidDataException("buttonType은 DivisionConst가 될 수 없다.");
            case < EAlarmButtonType.DivisionConst:
                InternalUpdateButtonColor(eAlarmButtonType, _alarmAudioClipButtons);
                break;
            case > EAlarmButtonType.DivisionConst:
                InternalUpdateButtonColor(eAlarmButtonType, _alarmTimeButtons);
                return;
        }
    }

    private void InternalUpdateButtonColor(EAlarmButtonType clickedButtonType, List<UIAlarmButton> list)
    {
        foreach (var widget in list)
        {
            if (widget.AlarmButtonType != clickedButtonType && widget.IsSelected)
            {
                widget.UpdateAlarmButton(false);
                continue;
            }

            if (widget.AlarmButtonType == clickedButtonType && !widget.IsSelected)
            {
                widget.UpdateAlarmButton(true);
            }
        }
    }

    #endregion

    #region 6. Methods

    // 

    #endregion
}