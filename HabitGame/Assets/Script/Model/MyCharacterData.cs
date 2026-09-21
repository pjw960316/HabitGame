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
        [XmlArrayItem("boolean")] public List<bool> RoutineCheckList = new();
    }

    public class SiestaTimeRecordData
    {
        public string Date;
        public int TotalSiestaMinutes;
    }

    #region 1. Fields

    public const int MONEY_PER_ROUTINE_SUCCESS = 500;
    public const int MONEY_PER_SIESTA_MINUTE = 10;

    private int _name;
    private int _age;
    private int _monthlyRoutineSuccessMoney;

    public List<RoutineRecordData> RoutineRecordList = new();
    public List<SiestaTimeRecordData> SiestaTimeRecordList = new();

    private Dictionary<string, List<bool>> _routineRecordDictionary = new();
    private Dictionary<string, int> _siestaTimeRecordDictionary = new();

    #endregion

    #region 2. Properties

    public string Name { get; set; }
    public int Age { get; set; }
    public int MonthlyRoutineSuccessMoney { get; set; }

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

    [XmlIgnore]
    public ImmutableDictionary<string, int> SiestaTimeRecordDictionary =>
        _siestaTimeRecordDictionary.ToImmutableDictionary();

    #endregion

    #region 3. Constructor

    // NOTE
    // MyCharacterManager에게 SetData 당한 이후에 호출된다.
    public void Initialize()
    {
        InitializeRoutineRecordDictionary();
        InitializeSiestaTimeRecordDictionary();
    }

    // TODO
    // 사용자 저장 XML의 필드 추가나 변경에 대응할 데이터 버전 및 마이그레이션 시스템이 필요하다.

    private void InitializeRoutineRecordDictionary()
    {
        var routineRecordList = RoutineRecordList
            .OrderByDescending(x => x.Date);

        foreach (var routineRecordData in routineRecordList)
        {
            var key = routineRecordData.Date;
            if (!IsValidRecordDate(key, nameof(RoutineRecordData)))
            {
                continue;
            }

            var routineCheckList = routineRecordData.RoutineCheckList;
            var list = new List<bool>();
            
            foreach (var routineCheck in routineCheckList)
            {
                list.Add(routineCheck);
            }

            _routineRecordDictionary[key] = list;
        }
    }

    private void InitializeSiestaTimeRecordDictionary()
    {
        foreach (var siestaTimeRecordData in SiestaTimeRecordList)
        {
            if (!IsValidRecordDate(siestaTimeRecordData.Date, nameof(SiestaTimeRecordData)))
            {
                continue;
            }

            _siestaTimeRecordDictionary[siestaTimeRecordData.Date] = siestaTimeRecordData.TotalSiestaMinutes;
        }
    }

    // WARNING
    // 날짜 누락으로 인한 초기화 예외만 방지하며, 구형 XML의 <Key>를 <Date>로 복구하지 않는다.
    // 건너뛴 루틴 기록은 SynchronizeDictionaryAndList 이후 저장하면 XML에서도 제외된다.
    // 구형 XML의 <Key>는 역직렬화 시 무시되므로, 다시 저장하기 전에 원본 XML을 별도로 백업해야 한다.
    private bool IsValidRecordDate(string date, string recordType)
    {
        if (string.IsNullOrWhiteSpace(date))
        {
            Debug.LogWarning($"{recordType}의 Date가 비어 있어 기록을 건너뜁니다. 저장 XML의 <Date> 태그를 확인하세요. 구형 <Key> 데이터는 자동 복구되지 않습니다.");
            return false;
        }

        return true;
    }

    #endregion

    #region 4. EventHandlers

    // 

    #endregion

    #region 5. Methods

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
                reward += MONEY_PER_ROUTINE_SUCCESS;
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

    // WARNING
    // 잊지 말고 List도 갱신시켜야 합니다.
    public void UpdateSiestaTime(TimeSpan curSiestaTime)
    {
        var totalSiestaMinutes = (int)curSiestaTime.TotalMinutes;
        if (totalSiestaMinutes == 0)
        {
            totalSiestaMinutes = 1;
        }
        
        var key = DateTime.Now.ToString("yyyyMMdd");
        if (_siestaTimeRecordDictionary.TryGetValue(key, out var prevSiestaMinutes))
        {
            totalSiestaMinutes += prevSiestaMinutes;
        }
        
        SiestaTimeRecordList.RemoveAll(siestaTimeRecord => siestaTimeRecord.Date == key);
        SiestaTimeRecordList.Add(new SiestaTimeRecordData
        {
            Date = key,
            TotalSiestaMinutes = totalSiestaMinutes
        });

        _siestaTimeRecordDictionary[key] = totalSiestaMinutes;
    }

    public void LogSiestaTimeRecordList()
    {
        foreach (var siestaTimeRecord in SiestaTimeRecordList)
        {
            Debug.Log($"낮잠 기록 날짜: {siestaTimeRecord.Date}, 낮잠 시간: {siestaTimeRecord.TotalSiestaMinutes}분");
        }
    }

    #endregion
}
