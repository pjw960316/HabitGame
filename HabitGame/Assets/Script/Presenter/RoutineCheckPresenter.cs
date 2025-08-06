using UniRx;

public class RoutineCheckPresenter : PresenterBase
{
    #region 1. Fields

    private UIRoutineCheckPopup _uiRoutineCheckPopup;
    private MyCharacterManager _myCharacterManager;

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    public sealed override void Initialize(IView view)
    {
        base.Initialize(view);

        _uiRoutineCheckPopup = _view as UIRoutineCheckPopup;
        _myCharacterManager = MyCharacterManager.Instance;

        ExceptionHelper.CheckNullException(_uiRoutineCheckPopup, "_uiRoutineCheckPopup");

        BindEvent();
    }

    #endregion

    #region 4. Methods

    private void BindEvent()
    {
        _uiRoutineCheckPopup.OnConfirmed.Subscribe(_ => HandleToggleEvent()).AddTo(_disposable);
    }

    #endregion

    #region 5. EventHandlers

    // 뭐가 눌렸는지 알아야 한다.
    private void HandleToggleEvent()
    {
        var toggleList = _uiRoutineCheckPopup.GetToggleList();
        var routineSuccessRewardMoney = _myCharacterManager.GetRoutineSuccessRewardMoney();
        int totalReward = 0;

        foreach (var toggleWidget in toggleList)
        {
            var toggle = toggleWidget.GetToggle();

            if (toggle.isOn)
            {
                totalReward += routineSuccessRewardMoney;
            }
        }

        RequestUpdateBudget(totalReward);
    }

    private void RequestUpdateBudget(int totalReward)
    {
        _myCharacterManager.UpdateBudget(totalReward);
    }

    #endregion
}