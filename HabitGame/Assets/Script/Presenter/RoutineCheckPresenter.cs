using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniRx;

// note
// 2개의 view를 1개의 presenter로 관리하는 방식
public class RoutineCheckPresenter : PresenterBase
{
    #region 1. Fields

    // note
    // view
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

        BindEvent();
    }

    #endregion

    #region 4. Methods

    private void BindEvent()
    {
        _uiRoutineCheckPopup.OnAwakeRoutineCheckPopup.Subscribe(_ => InitializeRoutineCheckPopup()).AddTo(_disposable);
        _uiRoutineCheckPopup.OnConfirmed.Subscribe(_ => HandleToggleEvent()).AddTo(_disposable);
    }
    
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
        var todayCompletedRoutineIndex = RequestGetTodayCompletedRoutineIndex(dateTime);

        _uiRoutineCheckPopup.InitializeToggle(todayCompletedRoutineIndex);
    }

    #endregion

    #region 5. Request Methods

    private async UniTaskVoid RequestUpdateBudgetAsync(int totalReward)
    {
        var serverResult = await _serverManager.RequestServerValidation(totalReward);

        if (serverResult == EServerResult.SUCCESS)
        {
            _myCharacterManager.UpdateCurrentRoutineSuccessRewardMoney(totalReward, DateTime.Now);

            RequestShowToast();
        }
    }

    private List<int> RequestGetTodayCompletedRoutineIndex(DateTime dateTime)
    {
        return _myCharacterManager.GetTodayCompletedRoutineIndex(dateTime);
    }

    private void RequestShowToast()
    {
        _uiToastManager.ShowToast(EToastStringKey.ERoutineCheckConfirm,
            _myCharacterManager.GetCurrentRoutineSuccessRewardMoney());
    }

    #endregion

    #region 6. EventHandlers

    private void HandleToggleEvent()
    {
        var toggleList = _uiRoutineCheckPopup.GetToggleList();
        var rewardPerRoutineSuccess = _myCharacterManager.GetRewardPerRoutineSuccess();
        var totalReward = 0;

        foreach (var toggleWidget in toggleList)
        {
            var toggle = toggleWidget.GetToggle();

            // todo : 이거 로직 바꿀 거임. 이미 한 거에 대한...
            // success routine
            if (toggle.isOn)
            {
                totalReward += rewardPerRoutineSuccess;
            }
        }

        RequestUpdateBudgetAsync(totalReward).Forget();
    }

    #endregion
}