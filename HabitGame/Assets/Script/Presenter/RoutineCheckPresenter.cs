using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

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

        RequestUpdateBudgetAsync(totalReward).Forget();

        RequestShowToast();
    }

    private async UniTaskVoid RequestUpdateBudgetAsync(int totalReward)
    {
        var a = _myCharacterManager.GetRoutineSuccessRewardMoney();
        Debug.Log($"routine money 1 : {a}");
        
        var serverResult = await _serverManager.RequestServerValidation(totalReward);

        if (serverResult == EServerResult.SUCCESS)
        {
            Debug.Log("server success");
            
            
            //test
            //1. MyCharcater Data Update
            //2. XML 데이터 serialize하기.
            //3. xml 바뀌었는 지 확인해라.
            _myCharacterManager.UpdateRoutineSuccessRewardMoney(totalReward);
            DataManager.Instance.UpdateData();
        }
    }

    private void RequestShowToast()
    {
        _uiToastManager.ShowToast(EToastStringKey.ERoutineCheckConfirm);
    }

    #endregion
}