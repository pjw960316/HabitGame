using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

[XmlRoot]
public class MyCharacterData
{
    public class RoutineRecordData
    {
        public string Date;
        [XmlArrayItem("boolean")]
        public List<bool> RoutineCheckList = new();
    }

    public class SiestaTimeRecordData
    {
        public string Date;
        public int TotalSiestaMinutes;
    }

    #region 1. Fields

    private int _name;
    private int _age;
    private int _monthlyRoutineSuccessMoney;
    private int _moneyPerRoutineSuccess;
    private TimeSpan _curSiestaTime;

    public List<RoutineRecordData> RoutineRecordList = new();
    public List<SiestaTimeRecordData> SiestaTimeRecordList = new();
    
    private Dictionary<string, List<bool>> _routineRecordDictionary = new();

    #endregion

    #region 2. Properties

    public string Name { get; set; }
    public int Age { get; set; }
    public int MonthlyRoutineSuccessMoney { get; set; }
    public int MoneyPerRoutineSuccess { get; set; }

    [XmlIgnore]
    public ImmutableSortedDictionary<string, ImmutableList<bool>> RoutineRecordDictionary
    {
        get
        {
            return _routineRecordDictionary
                .ToImmutableSortedDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.ToImmutableList(),
                    Comparer<string>.Create((a, b) => string.CompareOrdinal(b, a))
                );
        }
    }

    #endregion

    #region 3. Constructor
    // 
    #endregion

    #region 4. EventHandlers
    // 
    #endregion
    
    #region 5. Methods

    public void InitializeRoutineRecordDictionary()
    {
        var routineRecordList = RoutineRecordList
            .OrderByDescending(x => x.Date);

        foreach (var routineRecordData in routineRecordList)
        {
            var key = routineRecordData.Date;
            var routineCheckList = routineRecordData.RoutineCheckList;

            //shallow copy
            var list = new List<bool>();
            foreach (var routineCheck in routineCheckList) list.Add(routineCheck);

            _routineRecordDictionary[key] = list;
        }
    }

    public void UpdateRoutineRecordDictionary(List<int> todaySuccessfulRoutineIndexByView, DateTime dateTime)
    {
        var key = dateTime.ToString("yyyyMMdd");

        if (!_routineRecordDictionary.TryGetValue(key, out var todayRoutineRecordList))
        {
            // NOTE
            // 없으면 default 생성
            Debug.LogWarning($"Key가 없어서 \nkey가 {key}인 default routineRecord를 \nDictionary에 추가했다.");

            todayRoutineRecordList = new List<bool> { false, false, false, false };
            _routineRecordDictionary.Add(key, todayRoutineRecordList);
        }

        // NOTE
        // View에서 성공한 Index를 받아왔는데,
        // 기존의 todayRoutineRecordList가 false면 이번 이벤트에서 유저가 체크한 것이므로 갱신.
        var reward = 0;
        foreach (var index in todaySuccessfulRoutineIndexByView)
            if (!todayRoutineRecordList[index])
            {
                todayRoutineRecordList[index] = true;
                reward += MoneyPerRoutineSuccess;
            }

        UpdateMonthlyRoutineSuccessMoney(reward);
    }

    public void SynchronizeDictionaryAndList()
    {
        RoutineRecordList.Clear();

        foreach (var kvp in _routineRecordDictionary)
            RoutineRecordList.Add(new RoutineRecordData
            {
                Date = kvp.Key,
                RoutineCheckList = new List<bool>(kvp.Value)
            });
    }

    private void UpdateMonthlyRoutineSuccessMoney(int reward)
    {
        MonthlyRoutineSuccessMoney += reward;
    }

    public void UpdateSiestaTime(TimeSpan timeSpan)
    {
        _curSiestaTime = timeSpan;
    }

    private void TestSiestaTime()
    {
        Debug.Log(SiestaTimeRecordList[0]);
    }

    #endregion
}
