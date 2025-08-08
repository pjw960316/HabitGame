using System.Collections.Generic;

public class MyCharacterManager : ManagerBase<MyCharacterManager>, IManager
{
    #region 1. Fields

    private MyCharacterData _myCharacterData;
    private DataManager _dataManager;
    private MockServerManager _serverManager;

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    public void Initialize()
    {
        _dataManager = DataManager.Instance;
        _serverManager = MockServerManager.Instance;
        
        ExceptionHelper.CheckNullExceptionWithMessage(_dataManager, "DataManager" , "\n GameStartManager Initialize() makes critical Error!");
    }

    #endregion

    #region 4. Methods

    public void SetModel(IEnumerable<IModel> models)
    {
        foreach (var model in models)
        {
            if (model is MyCharacterData myCharacterData)
            {
                _myCharacterData = myCharacterData;
                break;
            }
        }

        ExceptionHelper.CheckNullException(_myCharacterData, "_myCharacterData in MyCharacterManager");
    }

    public int GetBudget()
    {
        return _myCharacterData.Budget;
    }

    public int GetRoutineSuccessRewardMoney()
    {
        return _myCharacterData.RoutineSuccessRewardMoney;
    }

    // todo 
    // 네이밍
    public void UpdateBudget(int changedBudget)
    {
        if (changedBudget == 0)
        {
            return;
        }
    }

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}