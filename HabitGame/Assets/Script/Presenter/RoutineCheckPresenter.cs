using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UniRx;

public class RoutineCheckPresenter : PresenterBase
{
    #region 1. Fields

    private UIRoutineCheckPopup _uiRoutineCheckPopup;

    private MyCharacterManager _myCharacterManager;
    private UIToastManager _uiToastManager;
    private MockServerManager _serverManager;

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    public sealed override void Initialize(IView view)
    {
        base.Initialize(view);

        _uiRoutineCheckPopup = _view as UIRoutineCheckPopup;
        ExceptionHelper.CheckNullException(_uiRoutineCheckPopup, "_uiRoutineCheckPopup");

        _myCharacterManager = MyCharacterManager.Instance;
        _uiToastManager = UIToastManager.Instance;
        _serverManager = MockServerManager.Instance;

        SetView();
        
        BindEvent();
    }

    protected sealed override void SetView()
    {
    }

    private void BindEvent()
    {
        _uiRoutineCheckPopup.OnAwakeRoutineCheckPopup.Subscribe(_ => InitializeRoutineCheckPopup()).AddTo(_disposable);
        _uiRoutineCheckPopup.OnConfirmed.Subscribe(_ => HandleToggleEvent()).AddTo(_disposable);
    }
    
    #endregion

    #region 4. Methods

    

    private void InitializeRoutineCheckPopup()
    {
        UpdateDateTextPerSecond();

        InitializeRoutineCheckToggle(DateTime.Now);
    }

    private void UpdateDateTextPerSecond()
    {
        Observable.Interval(TimeSpan.FromSeconds(1))
            .Subscribe(_ => { _uiRoutineCheckPopup.UpdateDateText(DateTime.Now); });
    }

    private void InitializeRoutineCheckToggle(DateTime dateTime)
    {
        var todaySuccessfulRoutineIndex = RequestGetTodaySuccessfulRoutineIndex(dateTime);

        // note
        // null이면 해당 루틴이 없는 것 이므로
        // Toggle을 그냥 냅두면 된다.
        if (todaySuccessfulRoutineIndex == null)
        {
            return;
        }
        
        _uiRoutineCheckPopup.InitializeToggle(todaySuccessfulRoutineIndex);
    }

    #endregion

    #region 5. Request Methods

    private async UniTaskVoid RequestUpdateRoutineRecordAsync(List<int> todaySuccessfulRoutineIndex, DateTime dateTime)
    {
        var serverResult = await _serverManager.RequestServerValidation();

        if (serverResult == EServerResult.SUCCESS)
        {
            _myCharacterManager.UpdateRoutineRecord(todaySuccessfulRoutineIndex, dateTime);

            RequestShowToast();
        }
    }

    [CanBeNull]
    private List<int> RequestGetTodaySuccessfulRoutineIndex(DateTime dateTime)
    {
        return _myCharacterManager.GetTodaySuccessfulRoutineIndex(dateTime);
    }

    private void RequestShowToast()
    {
        _uiToastManager.ShowToast(EToastStringKey.ERoutineCheckConfirm,
            _myCharacterManager.GetMonthlyRoutineSuccessMoney());
    }

    #endregion

    #region 6. EventHandlers

    private void HandleToggleEvent()
    {
        var toggleList = _uiRoutineCheckPopup.GetToggleList();
        var todaySuccessfulRoutineIndex = new List<int>();
        var toggleListCount = toggleList.Count;

        for (var index = 0; index < toggleListCount; index++)
        {
            var toggle = toggleList[index];
            var isToggleChecked = toggle.GetToggle().isOn;

            if (isToggleChecked)
            {
                todaySuccessfulRoutineIndex.Add(index);
            }
        }

        RequestUpdateRoutineRecordAsync(todaySuccessfulRoutineIndex, DateTime.Now).Forget();
    }

    #endregion
}