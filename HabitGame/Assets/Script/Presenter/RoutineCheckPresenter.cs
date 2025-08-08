using UniRx;

public class RoutineCheckPresenter : PresenterBase
{
    #region 3. Constructor

    public sealed override void Initialize(IView view)
    {
        base.Initialize(view);

        _uiRoutineCheckPopup = _view as UIRoutineCheckPopup;
        _myCharacterManager = MyCharacterManager.Instance;
        _uiToastManager = UIToastManager.Instance;

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

    #region 1. Fields

    private UIRoutineCheckPopup _uiRoutineCheckPopup;
    private MyCharacterManager _myCharacterManager;
    private UIToastManager _uiToastManager;

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 5. EventHandlers

    private void HandleToggleEvent()
    {
        var toggleList = _uiRoutineCheckPopup.GetToggleList();
        var routineSuccessRewardMoney = _myCharacterManager.GetRoutineSuccessRewardMoney();
        var totalReward = 0;

        foreach (var toggleWidget in toggleList)
        {
            var toggle = toggleWidget.GetToggle();

            if (toggle.isOn)
            {
                totalReward += routineSuccessRewardMoney;
            }
        }

        RequestUpdateBudget(totalReward);

        RequestShowToast();
    }

    private void RequestUpdateBudget(int totalReward)
    {
        _myCharacterManager.UpdateBudget(totalReward);
    }

    private void RequestShowToast()
    {
        _uiToastManager.ShowToast(EToastStringKey.ERoutineCheckConfirm);
    }

    #endregion
}