using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class UIRoutineCheckPopup : UIPopupBase
{
    #region 1. Fields

    [SerializeField] private List<UIToggleBase> _toggleList = new();
    [SerializeField] private UIButtonBase _confirmButton;

    private readonly Subject<Unit> _onConfirmed = new();
    public IObservable<Unit> OnConfirmed => _onConfirmed;

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    public override void OnAwake()
    {
        base.OnAwake();

        CreatePresenterByManager();

        BindEvent();
    }

    #endregion

    #region 4. Methods

    private void BindEvent()
    {
        _confirmButton.Button.onClick.AddListener(() =>
        {
            _onConfirmed.OnNext(default);

            //refactor
            Destroy(this);
        });
    }

    public override void CreatePresenterByManager()
    {
        _uiManager.CreatePresenter<RoutineCheckPresenter>(this);
    }

    public List<UIToggleBase> GetToggleList()
    {
        return _toggleList;
    }

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}