using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Xml.Serialization;
using UnityEngine;

[XmlRoot]
public class MyCharacterData : IModel
{
    public class RoutineRecordData
    {
        public string Key;

        [XmlArrayItem("boolean")] public List<bool> RoutineCheckList = new();
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

    [XmlIgnore]
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
            var key = routineRecordData.Key;
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

        if (_routineRecordDictionary.TryGetValue(key, out var todayRoutineRecordList) == false)
        {
            // note
            // 없으면 default 생성
            Debug.Log($"Key가 없어서 \nkey가 {key}인 default routineRecord를 \nDictionary에 추가했다.");

            todayRoutineRecordList = new List<bool> { false, false, false, false };
            _routineRecordDictionary.Add(key, todayRoutineRecordList);
        }

        // note
        // View에서 성공한 Index를 받아왔는데,
        // 기존의 todayRoutineRecordList가 false면 이번 이벤트에서 유저가 체크한 것이므로 갱신.
        var reward = 0;
        foreach (var index in todaySuccessfulRoutineIndexByView)
        {
            if (todayRoutineRecordList[index] == false)
            {
                todayRoutineRecordList[index] = true;
                reward += MoneyPerRoutineSuccess;
            }
        }

        UpdateMonthlyRoutineSuccessMoney(reward);
    }

    public void SynchronizeDictionaryAndList()
    {
        RoutineRecordList.Clear();

        foreach (var kvp in _routineRecordDictionary)
        {
            RoutineRecordList.Add(new RoutineRecordData
            {
                Key = kvp.Key,
                RoutineCheckList = new List<bool>(kvp.Value)
            });
        }
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