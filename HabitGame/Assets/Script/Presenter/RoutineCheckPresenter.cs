using Cysharp.Threading.Tasks;
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

    private void HandleToggleEvent()
    {
        var toggleList = _uiRoutineCheckPopup.GetToggleList();
        var rewardPerRoutineSuccess = _myCharacterManager.GetRewardPerRoutineSuccess();
        var totalReward = 0;

        foreach (var toggleWidget in toggleList)
        {
            var toggle = toggleWidget.GetToggle();

            // success routine
            if (toggle.isOn)
            {
                totalReward += rewardPerRoutineSuccess;
            }
        }

        RequestUpdateBudgetAsync(totalReward).Forget();
    }

    private async UniTaskVoid RequestUpdateBudgetAsync(int totalReward)
    {
        var serverResult = await _serverManager.RequestServerValidation(totalReward);

        if (serverResult == EServerResult.SUCCESS)
        {
            _myCharacterManager.UpdateCurrentRoutineSuccessRewardMoney(totalReward);

            RequestShowToast();
        }
    }

    private void RequestShowToast()
    {
        _uiToastManager.ShowToast(EToastStringKey.ERoutineCheckConfirm,
            _myCharacterManager.GetCurrentRoutineSuccessRewardMoney());
    }

    #endregion
}