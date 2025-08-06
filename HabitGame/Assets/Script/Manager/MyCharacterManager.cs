using System;
using System.Collections.Generic;
using UniRx;

public class MyCharacterManager : ManagerBase<MyCharacterManager>, IManager
{
    #region 1. Fields

    private MyCharacterData _myCharacterData;

    private readonly Subject<Unit> _onUpdateBudget = new();
    private IObservable<Unit> OnUpdateBudget => _onUpdateBudget;

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    public void Initialize()
    {
        
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
        
        ExceptionHelper.CheckNullException(_myCharacterData , "_myCharacterData in MyCharacterManager");
    }

    public int GetBudget()
    {
        return _myCharacterData.Budget;
    }

    public int GetRoutineSuccessRewardMoney()
    {
        return _myCharacterData.RoutineSuccessRewardMoney;
    }
    
    public void UpdateBudget(int changedBudget)
    {
        _myCharacterData.Budget += changedBudget;
    }

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}