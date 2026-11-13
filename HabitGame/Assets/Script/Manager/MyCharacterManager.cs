using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
    private DrinkingWaterHandler _drinkingWaterHandler;

    private readonly Subject<Unit> _onUpdateRoutineSuccess = new();
    private readonly Subject<int> _onUpdateSiestaRecord = new();

    #endregion

    #region 2. Properties

    public Subject<Unit> OnUpdateRoutineSuccess => _onUpdateRoutineSuccess;
    public IObservable<int> OnUpdateSiestaRecord => _onUpdateSiestaRecord;

    #endregion

    #region 3. Constructor

    public sealed override void Initialize()
    {
        _xmlDataManager = XmlDataManager.Instance;
        ExceptionHelper.CheckNullException(_xmlDataManager, "_xmlDataManager");
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
        _drinkingWaterHandler = new DrinkingWaterHandler(_myCharacterData);
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

    public int GetMoneyPerWaterDrinking()
    {
        return MyCharacterData.MONEY_PER_WATER_DRINKING;
    }

    public int GetMonthlyWaterPointRewardCount()
    {
        return _drinkingWaterHandler.GetMonthlyWaterPointRewardCount();
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

    private sealed class DrinkingWaterHandler
    {
        private readonly MyCharacterData _myCharacterData;

        public DrinkingWaterHandler(MyCharacterData myCharacterData)
        {
            _myCharacterData = myCharacterData;
            
            UpdateLatestWaterDrinkingDate();

            UpdateDrinkingWaterPoint();
        }

        public int GetMonthlyWaterPointRewardCount()
        {
            var currentYearMonth = DateTimeHelper.ToYearMonth(DateTime.Now);
            var monthlyWaterPointRecordDictionary = _myCharacterData.MonthlyWaterPointRecordDictionary;

            if (!monthlyWaterPointRecordDictionary.TryGetValue(currentYearMonth, out var rewardCount))
            {
                // WARNING : 없어도 계산을 위해 0을 리턴해줘야 한다!
                return 0;
            }

            return rewardCount;
        }

        // NOTE
        // 안드로이드로 부터 받은 최신 물 섭취 날짜를 Data에 갱신
        private void UpdateLatestWaterDrinkingDate()
        {
            var latestWaterDrinkingTimestamp = AndroidManager.Instance.GetLatestWaterDrinkingDate();
            var latestWaterDrinkingDateFromAndroid = ConvertToWaterDrinkingDate(latestWaterDrinkingTimestamp);
            
            _myCharacterData.UpdateLatestWaterDrinkingDateFromAndroid(latestWaterDrinkingDateFromAndroid);
        }

        // WARNING
        // 이게 한 번만 호출되도록 설계
        private DateTime ConvertToWaterDrinkingDate(long unixTimeMilliseconds)
        {
            if (unixTimeMilliseconds <= 0L)
            {
                return DateTime.MinValue;
            }

            return DateTimeOffset.FromUnixTimeMilliseconds(unixTimeMilliseconds).LocalDateTime.Date;
        }
        
        private void UpdateDrinkingWaterPoint()
        {
            var latestWaterDrinkingDate = _myCharacterData.LatestWaterDrinkingDateFromAndroid.Date;
            var latestRewardDate = _myCharacterData.LatestWaterPointRewardDate.Date;

            if (latestWaterDrinkingDate == DateTime.MinValue)
            {
                return;
            }

            // NOTE
            // 물 마시고 앱 푸쉬를 누른 날은 20261025이고 
            // 가장 최근에 보상을 받은 날도 20261025면 포인트를 지급하지 않는다.
            if (latestWaterDrinkingDate == latestRewardDate)
            {
                return;
            }

            // NOTE
            // 월별 물 마신 자료구조 갱신 -> 그리고 물 마신 날짜 2개 동기화
            _myCharacterData.IncrementMonthlyWaterPointRewardCount(latestWaterDrinkingDate);
            _myCharacterData.SynchronizeWaterDrinkingDates();

            // NOTE
            // Deserialize 이 후 최신화가 된다.
            
            // WARNING : 일단 여기서 Serialize()까지 해버리자.  ->  반드시 변경해야 함. 아직 내가 정책을 만들지 않아서 그럼.
            MyCharacterManager.Instance.UpdateXmlData();
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
