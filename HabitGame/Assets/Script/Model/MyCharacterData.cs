using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot]
public class MyCharacterData : IModel
{
    public class RoutineRecordData
    {
        public int key;
        public bool RoutineOneCheck;
        public bool RoutineTwoCheck;
        public bool RoutineThreeCheck;
        public bool RoutineFourCheck;
    }

    #region 1. Fields

    private int _name;
    private int _age;
    private int _currentRoutineSuccessRewardMoney;
    private int _rewardPerRoutineSuccess;

    [XmlIgnore] private Dictionary<string, List<bool>> _routineRecordDictionary = new();

    //test
    //뒤에 계속 추가
    //데이터 갱신 - key로
    //데이터 
    [XmlIgnore] //test
    private List<RoutineRecordData> _routineRecordData = new();

    #endregion

    #region 2. Properties

    public string Name { get; set; }
    public int Age { get; set; }
    public int CurrentRoutineSuccessRewardMoney { get; set; }
    public int RewardPerRoutineSuccess { get; set; }

    [XmlIgnore]
    public Dictionary<string, List<bool>> RoutineRecordDictionary
    {
        get => _routineRecordDictionary;
        private set => _routineRecordDictionary = value;
    }

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