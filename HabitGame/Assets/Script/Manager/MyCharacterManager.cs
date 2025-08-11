using System.Collections.Generic;
using UnityEngine;

public class MyCharacterManager : ManagerBase<MyCharacterManager>, IManager
{
    #region 1. Fields

    private MyCharacterData _myCharacterData;
    private DataManager _dataManager;

    #endregion

    #region 2. Properties
    
    // property
    //test
    public MyCharacterData MyCharacterData => _myCharacterData;

    #endregion

    #region 3. Constructor

    public void Initialize()
    {
        _dataManager = DataManager.Instance;
        
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

    //test
    //set이 있는데 이게 괜찮은지...
    public void UpdateRoutineSuccessRewardMoney(int totalReward)
    {
        _myCharacterData.RoutineSuccessRewardMoney += totalReward;
        
        Debug.Log($"클라이언트에서 RoutineSuccessRewardMoney {_myCharacterData.RoutineSuccessRewardMoney}");
    }

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}