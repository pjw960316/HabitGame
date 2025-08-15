using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Collections.Immutable;

[XmlRoot]
public class MyCharacterData : IModel
{
    public class RoutineRecordData
    {
        public string key;
        public List<bool> RoutineCheckList = new();
    }
    
    #region 1. Fields

    private int _name;
    private int _age;
    private int _monthlyRoutineSuccessMoney; // Note : 한 달 동안 루틴을 성공해서 번 총 금액 -> 다음 달 나에게 주는 선물의 총액
    private int _moneyPerRoutineSuccess; // Note : 루틴 1개 success 당 얻는 돈
    public List<RoutineRecordData> RoutineRecordList = new();
    
    [XmlIgnore] private Dictionary<string, List<bool>> _routineRecordDictionary = new();

    #endregion

    #region 2. Properties

    public string Name { get; set; }
    public int Age { get; set; }
    public int MonthlyRoutineSuccessMoney { get; set; }
    public int MoneyPerRoutineSuccess { get; set; }

    public ImmutableDictionary<string, ImmutableList<bool>> RoutineRecordDictionary
    {
        get
        {
            return _routineRecordDictionary.ToImmutableDictionary
            (
                kvp => kvp.Key,
                kvp => kvp.Value.ToImmutableList()
            );
        }
    }
    #endregion

    #region 3. Constructor

    // default

    #endregion

    #region 4. Methods

    // note
    // XML에서 Load한 List<RoutineRecordData>을
    // Dictionary로 가공
    public void InitializeRoutineRecordDictionary()
    {
        foreach (var routineRecordData in RoutineRecordList)
        {
            var key = routineRecordData.key;
            var routineCheckList = routineRecordData.RoutineCheckList;

            //shallow copy
            var list = new List<bool>();
            foreach (var routineCheck in routineCheckList)
            {
                list.Add(routineCheck);
            }
            
            _routineRecordDictionary[key] = list;
        }
    }

    public void UpdateRoutineRecordDictionary(List<int> todaySuccessfulRoutineIndexByView, DateTime dateTime)
    {
        var key = dateTime.ToString("yyyyMMdd");

        // note
        // 오늘의 루틴 완료 여부를 0번 루틴 ~ 마지막 루틴 까지 bool로 저장
        var todayRoutineRecordList = _routineRecordDictionary[key];

        // refactor
        // reward 는 두 가지 기능이 포함된 거
        // 분할 요망
        var reward = 0;
        foreach (var index in todaySuccessfulRoutineIndexByView)
        {
            // note
            // 기존에 false 였는데 유저가 체크했다면 얘는 최근에 수행했다는 뜻.
            if (todayRoutineRecordList[index] == false)
            {
                todayRoutineRecordList[index] = true;
                reward += MoneyPerRoutineSuccess;
            }
        }

        UpdateMonthlyRoutineSuccessMoney(reward);
    }
    
    private void UpdateMonthlyRoutineSuccessMoney(int reward)
    {
        MonthlyRoutineSuccessMoney += reward;
    }

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}