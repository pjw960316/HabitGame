using System.Collections.Generic;

public class MyCharacterManager : ManagerBase<MyCharacterManager>, IManager
{
    #region 1. Fields

    // Note
    // Model을 외부에서 접근하게 하지말고
    // 필요하면 argument로 전달해라
    private MyCharacterData _myCharacterData;
    private XmlDataSerializeManager _xmlDataSerializeManager;

    //

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    public void PreInitialize()
    {
        //
    }

    public void Initialize()
    {
        _xmlDataSerializeManager = XmlDataSerializeManager.Instance;

        ExceptionHelper.CheckNullExceptionWithMessage(_xmlDataSerializeManager, "xmlDataSerializeManager",
            "\n GameStartManager Initialize() makes critical Error!");
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

    public int GetRewardPerRoutineSuccess()
    {
        return _myCharacterData.RewardPerRoutineSuccess;
    }

    public int GetCurrentRoutineSuccessRewardMoney()
    {
        return _myCharacterData.CurrentRoutineSuccessRewardMoney;
    }

    public void UpdateCurrentRoutineSuccessRewardMoney(int totalReward)
    {
        _myCharacterData.CurrentRoutineSuccessRewardMoney += totalReward;

        RequestUpdateXmlData();
    }

    private void RequestUpdateXmlData()
    {
        _xmlDataSerializeManager.SerializeXmlData<MyCharacterData>(_myCharacterData);
    }

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}