using System;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class UIRoutineCheckPopup : UIPopupBase
{
    #region 1. Fields

    [SerializeField] private TextMeshProUGUI _dayText;
    [SerializeField] private List<UIToggleBase> _toggleList = new();
    [SerializeField] private UIButtonBase _confirmButton;

    private readonly Subject<Unit> _onAwakeRoutineCheckPopup = new();
    public IObservable<Unit> OnAwakeRoutineCheckPopup => _onAwakeRoutineCheckPopup;

    private readonly Subject<Unit> _onConfirmed = new();
    public IObservable<Unit> OnConfirmed => _onConfirmed;

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    protected override void OnAwake()
    {
        base.OnAwake();

        _onAwakeRoutineCheckPopup.OnNext(default);
    }

    protected sealed override void Initialize()
    {
        base.Initialize();

        UpdateDateText(DateTime.Now);
    }

    protected override void CreatePresenterByManager()
    {
        _uiManager.CreatePresenter<RoutineCheckPresenter>(this);
    }

    #endregion

    #region 4. Methods

    protected sealed override void BindEvent()
    {
        _confirmButton.OnClick.AddListener(OnClickConfirmButton);
    }

    #endregion


    #region 4. EventHandlers

    private void OnClickConfirmButton()
    {
        _onConfirmed.OnNext(default);

        ClosePopup();
    }

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    public void UpdateDateText(DateTime dateTime)
    {
        _dayText.SetText($"{dateTime}");
    }

    public List<UIToggleBase> GetToggleList()
    {
        return _toggleList;
    }

    public void SetToggle(List<int> todayCompletedRoutineIndex)
    {
        foreach (var index in todayCompletedRoutineIndex)
        {
            var toggle = _toggleList[index].GetToggle();

            toggle.interactable = false;
            toggle.isOn = true;
        }
    }

    #endregion
}