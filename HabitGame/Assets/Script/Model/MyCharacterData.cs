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
    private int _monthlyRoutineSuccessMoney; // Note : 한 달 동안 루틴을 성공해서 번 총 금액 -> 다음 달 나에게 주는 선물의 총액
    private int _moneyPerRoutineSuccess; // Note : 루틴 1개 success 당 얻는 돈

    [XmlIgnore] private Dictionary<string, List<bool>> _routineRecordDictionary = new();
    [XmlIgnore] private List<RoutineRecordData> _routineRecordData = new();

    #endregion

    #region 2. Properties

    public string Name { get; set; }
    public int Age { get; set; }
    public int MonthlyRoutineSuccessMoney { get; set; }
    public int MoneyPerRoutineSuccess { get; set; }

    // note
    // 날짜를 key - string
    // 4개의 루틴의 success 여부를 bool List
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