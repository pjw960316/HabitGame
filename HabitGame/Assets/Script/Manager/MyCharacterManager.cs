using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using JetBrains.Annotations;
using UniRx;

// NOTE : 추상화
// 게임 전체 (씬 무관)에서 MVP 객체들로 인해 플레이어 데이터의 변경을 관리한다.
// Event Call을 하나의 단위로 생각하고 -> MyCharacterData와 관련 XML을 모두 갱신한다.
// 1. MyCharacterData 만 갱신 -> 영구적이지 못하나 게임 진행 도중에는 빠르고 간편
// 2. Xml 전체 갱신 -> 영구 데이터를 갱신 그러나 Disk I/O 발생

public interface IUpdateAndSaveData
{
    public void UpdateMyCharacterData(TimeSpan siestaTime);
    public void SaveMyCharacterDataXML();
}

public class MyCharacterManager : ManagerBase<MyCharacterManager>
{
    #region 1. Fields

    private MyCharacterData _myCharacterData;
    private XmlDataManager _xmlDataManager;
    private SiestaHandler _siestaHandler;
    private WaterPointHandler _waterPointHandler;

    private readonly Subject<Unit> _onUpdateRoutineSuccess = new();
    private readonly Subject<int> _onUpdateSiestaRecord = new();

    #endregion

    #region 2. Properties

    public Subject<Unit> OnUpdateRoutineSuccess => _onUpdateRoutineSuccess;
    public IObservable<int> OnUpdateSiestaRecord => _onUpdateSiestaRecord;

    #endregion

    #region 3. Constructor

    public sealed override void PreInitialize()
    {
        _xmlDataManager = XmlDataManager.Instance;
        ExceptionHelper.CheckNullException(_xmlDataManager, "_xmlDataManager");
    }

    public sealed override void Initialize()
    {
        //
    }

    #endregion

    #region 4. EventHandlers

    // default

    #endregion

    // NOTE
    // Field & UI를 통해 플레이어 데이터의 변화가 생긴다.
    // 그들의 presenter는 Manager에게 이를 알린다. -> 그러므로 public으로 열어둔다.

    #region 5-1. public Update Methods for MVP Objects

    // TODO 
    // Siesta 처럼 수정
    public void UpdateRoutineRecord(List<int> todaySuccessfulRoutineIndexByView, DateTime dateTime)
    {
        UpdateRoutineRecordDictionary(todaySuccessfulRoutineIndexByView, dateTime);

        _onUpdateRoutineSuccess.OnNext(default);
        SynchronizeDictionaryAndList();
        UpdateXmlData();
    }

    public void UpdateSiestaRecord(TimeSpan siestaTime)
    {
        _siestaHandler.UpdateMyCharacterData(siestaTime);
        UpdateXmlData();

        _onUpdateSiestaRecord.OnNext(_siestaHandler.GetMonthlySiestaMoney());
        
        //_siestaHandler.SaveMyCharacterDataXML();
    }

    #endregion

    #region 5-2. Methods

    public sealed override void SetData()
    {
        // NOTE : 이 시점에 XML -> C# Class로 Deserialize가 완료된다.
        _myCharacterData = XmlDataManager.Instance.GetDeserializedXmlData<MyCharacterData>();
        ExceptionHelper.CheckNullException(_myCharacterData, "_myCharacterData in MyCharacterManager");
        
        _myCharacterData.Initialize();
        
        InitializeHandlers();
    }
    
    // WARNING
    // 일단 호출 시점 고민 안하고 만들긴 했다.
    // handler initialize 타이밍
    private void InitializeHandlers()
    {
        _siestaHandler = new SiestaHandler(_myCharacterData);
        _waterPointHandler = new WaterPointHandler(_myCharacterData);
    }

    [CanBeNull]
    public List<int> GetTodaySuccessfulRoutineIndex(DateTime dateTime)
    {
        var key = dateTime.ToString("yyyyMMdd");
        var immutableRoutineRecordDictionary = _myCharacterData.RoutineRecordDictionary;

        if (!immutableRoutineRecordDictionary.TryGetValue(key, out var immutableTodayRecordList))
        {
            // NOTE
            // 첫 루틴 기록이므로 아직 기록이 없으므로
            // null return은 의도된 것.

            return null;
        }

        var successfulRoutineIndex = new List<int>();
        for (var index = 0; index < immutableTodayRecordList.Count; index++)
        {
            if (immutableTodayRecordList[index])
            {
                successfulRoutineIndex.Add(index);
            }
        }

        return successfulRoutineIndex;
    }

    // NOTE
    // 최신 날짜가 맨 앞에
    [NotNull]
    public ImmutableSortedDictionary<string, ImmutableList<bool>> GetRoutineRecordDictionary()
    {
        return _myCharacterData.RoutineRecordDictionary;
    }


    public int GetMonthlyRoutineSuccessMoney()
    {
        return _myCharacterData.MonthlyRoutineSuccessMoney;
    }

    public int GetMoneyPerRoutineSuccess()
    {
        return MyCharacterData.MONEY_PER_ROUTINE_SUCCESS;
    }

    public int GetMonthlySiestaMoney()
    {
        return _siestaHandler.GetMonthlySiestaMoney();
    }

    public int GetTodaySiestaMoney()
    {
        return _siestaHandler.GetTodaySiestaMoney();
    }

    public int GetWaterPointsPerDay()
    {
        return MyCharacterData.WATER_POINTS_PER_DAY;
    }

    public int GetMonthlyWaterCheckCount()
    {
        return _waterPointHandler.GetMonthlyCheckCount();
    }

    public void LogSiestaTimeRecordList()
    {
        _myCharacterData.LogSiestaTimeRecordList();
    }

    private void UpdateXmlData()
    {
        _xmlDataManager.SerializeXmlData<MyCharacterData>(_myCharacterData);
    }

    private void UpdateRoutineRecordDictionary(List<int> todaySuccessfulRoutineIndexByView, DateTime dateTime)
    {
        _myCharacterData.UpdateRoutineRecordDictionary(todaySuccessfulRoutineIndexByView, dateTime);
    }

    private void SynchronizeDictionaryAndList()
    {
        _myCharacterData.SynchronizeDictionaryAndList();
    }

    #endregion

    private sealed class WaterPointHandler
    {
        private readonly MyCharacterData _myCharacterData;
        private readonly AndroidManager _androidManager;

        public WaterPointHandler(MyCharacterData myCharacterData)
        {
            _myCharacterData = myCharacterData;
            ExceptionHelper.CheckNullException(_myCharacterData, "_myCharacterData");

            _androidManager = AndroidManager.Instance;
            ExceptionHelper.CheckNullException(_androidManager, "_androidManager");
            
            var latestWithdrawnPointDate = _myCharacterData.LatestWithdrawnPointDate;
            
            // NOTE
            // 1. MyCharacterData에게 confirmedDates를 주고 그걸 기반으로 메모리에 초기화 시키도록
            
            // NOTE
            // 2. 미지급 포인트 횟수 계산
            var pendingCheckCount = CountPendingChecks(latestWithdrawnPointDate, confirmedDates);
            
            // NOTE
            // 3. 데이터에 최신 포인트 지급일 업데이트
            UpdateLastPointDate(lastPointDate, confirmedDates);
        }
        

        private int CountPendingChecks(DateTime lastPointDate, HashSet<string> confirmedDates)
        {
            var lastPointDateNumber = DateTimeHelper.ToDateNumber(lastPointDate);
            var count = 0;

            foreach (var confirmedDate in confirmedDates)
            {
                if (DateTime.TryParseExact(confirmedDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date) &&
                    DateTimeHelper.ToDateNumber(date) > lastPointDateNumber)
                {
                    count++;
                }
            }

            return count;
        }
        
        private void UpdateLastPointDate(DateTime lastPointDate, HashSet<string> confirmedDates)
        {
            var latestCheckDate = lastPointDate.Date;

            foreach (var confirmedDate in confirmedDates)
            {
                if (DateTime.TryParseExact(confirmedDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date) &&
                    date > latestCheckDate)
                {
                    latestCheckDate = date;
                }
            }

            if (latestCheckDate > lastPointDate.Date)
            {
                _myCharacterData.LastWaterPointDate = latestCheckDate;
            }
        }

        public int GetMonthlyCheckCount()
        {
            var yearMonth = DateTimeHelper.ToYearMonth(DateTime.Now);
            var checkCounts = _myCharacterData.WaterCheckCountsByMonth;

            if (!checkCounts.TryGetValue(yearMonth, out var checkCount))
            {
                // WARNING : 없어도 계산을 위해 0을 리턴해줘야 한다!
                return 0;
            }

            return checkCount;
        }
    }

    private sealed class SiestaHandler : IUpdateAndSaveData
    {
        private readonly MyCharacterData _myCharacterData;

        public SiestaHandler(MyCharacterData myCharacterData)
        {
            _myCharacterData = myCharacterData;
        }

        public void UpdateMyCharacterData(TimeSpan siestaTime)
        {
            _myCharacterData.UpdateSiestaTime(siestaTime);
        }

        public int GetMonthlySiestaMoney()
        {
            var currentYearMonth = DateTime.Now.ToString("yyyyMM");
            var siestaTimeRecordDictionary = _myCharacterData.SiestaTimeRecordDictionary;
            var totalSiestaMinutes = 0;

            foreach (var siestaTimeRecord in siestaTimeRecordDictionary)
            {
                if (siestaTimeRecord.Key.StartsWith(currentYearMonth, StringComparison.Ordinal))
                {
                    totalSiestaMinutes += siestaTimeRecord.Value;
                }
            }

            return totalSiestaMinutes * MyCharacterData.MONEY_PER_SIESTA_MINUTE;
        }

        public int GetTodaySiestaMoney()
        {
            var key = DateTime.Now.ToString("yyyyMMdd");
            var siestaTimeRecordDictionary = _myCharacterData.SiestaTimeRecordDictionary;

            if (!siestaTimeRecordDictionary.TryGetValue(key, out var totalSiestaMinutes))
            {
                return 0;
            }

            return totalSiestaMinutes * MyCharacterData.MONEY_PER_SIESTA_MINUTE;
        }

        // TODO
        // 이게 필요할까? 그냥 통합 업데이트가 맞는가?
        // C# -> XML
        public void SaveMyCharacterDataXML()
        {
            
        }
    }
}
