using System.Xml.Serialization;

[XmlRoot]
public class MyCharacterData : IModel
{
    #region 1. Fields

    private int _name;
    private int _age;
    private int _currentRoutineSuccessRewardMoney;
    private int _rewardPerRoutineSuccess;

    #endregion

    #region 2. Properties

    public string Name { get; set; }
    public int Age { get; set; }
    public int CurrentRoutineSuccessRewardMoney { get; set; }
    public int RewardPerRoutineSuccess { get; set; }

    #endregion

    #region 3. Constructor

    // default

    #endregion

    #region 4. Methods

    // default

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}