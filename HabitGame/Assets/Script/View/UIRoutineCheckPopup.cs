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

    public override void OnAwake()
    {
        base.OnAwake();

        CreatePresenterByManager();

        Initialize();
        
        BindEvent();
        
        _onAwakeRoutineCheckPopup.OnNext(default);
    }

    private void Initialize()
    {
        UpdateDateText(DateTime.Now);

        //Note
        //Toggle의 최초 상태는 OnAwakeRoutineCheckPopup에서 Initialize 된다.
    }

    #endregion

    #region 4. Methods

    private void BindEvent()
    {
        _confirmButton.Button.onClick.AddListener(() =>
        {
            _onConfirmed.OnNext(default);

            //refactor
            Destroy(gameObject);
        });
    }

    protected override void CreatePresenterByManager()
    {
        _uiManager.CreatePresenter<RoutineCheckPresenter>(this);
    }

    public List<UIToggleBase> GetToggleList()
    {
        return _toggleList;
    }

    public void UpdateDateText(DateTime dateTime)
    {
        _dayText.SetText($"{dateTime}");
    }

    public void UpdateToggle(List<int> completedTodayRoutineIndex)
    {
        //todo : 예외처리
        foreach (int index in completedTodayRoutineIndex)
        {
            _toggleList[index].GetToggle().isOn = true;
        }
    }

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}