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
    public void UpdateMyCharacterData();
    public void SaveMyCharacterDataXML();
}

public class MyCharacterManager : ManagerBase<MyCharacterManager>
{
    #region 1. Fields

    private MyCharacterData _myCharacterData;
    private XmlDataManager _xmlDataManager;

    private readonly Subject<Unit> _onUpdateRoutineSuccess = new();

    #endregion

    #region 2. Properties

    public Subject<Unit> OnUpdateRoutineSuccess => _onUpdateRoutineSuccess;

    #endregion

    #region 3. Constructor

    public sealed override void Initialize()
    {
        _xmlDataManager = XmlDataManager.Instance;
        ExceptionHelper.CheckNullException(_xmlDataManager, "_xmlDataSerializeManager");

        InitializeRoutineRecordDictionary();
    }

    #endregion
    
    #region 4. EventHandlers

    // default

    #endregion
    
    // NOTE
    // Field & UI를 통해 플레이어 데이터의 변화가 생긴다.
    // 그들의 presenter는 Manager에게 이를 알린다. -> 그러므로 public으로 열어둔다.
    #region 5-1. public Update Methods for MVP Objects

    public void UpdateRoutineRecord(List<int> todaySuccessfulRoutineIndexByView, DateTime dateTime)
    {
        UpdateRoutineRecordDictionary(todaySuccessfulRoutineIndexByView, dateTime);

        _onUpdateRoutineSuccess.OnNext(default);

        // TODO
        // 이걸 매번 할 필요는 없지?
        // 게임 종료시에만?
        SynchronizeDictionaryAndList();
        UpdateXmlData();
    }

    public void UpdateSiestaRecord(TimeSpan siestaTime)
    {
        
    }
    #endregion
    
    // NOTE
    // public Update Methods for MVP Objects을 통해 데이터를 받으면
    // 언제나 MyCharacterData 와 XML을 갱신해준다. 
    #region 5-2. private Methods for Update

    private void UpdateData()
    {
        UpdateMyCharacterData();
        SaveMyCharacterDataXML();
    }
    private void UpdateMyCharacterData()
    {
        
    }

    private void SaveMyCharacterDataXML()
    {
        
    }
    #endregion
    public sealed override void SetModel()
    {
        _myCharacterData = XmlDataManager.Instance.GetDeserializedXmlData<MyCharacterData>();
        ExceptionHelper.CheckNullException(_myCharacterData, "_myCharacterData in MyCharacterManager");
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
        return _myCharacterData.MoneyPerRoutineSuccess;
    }


    private void UpdateXmlData()
    {
        _xmlDataManager.SerializeXmlData<MyCharacterData>(_myCharacterData);
    }

    private void InitializeRoutineRecordDictionary()
    {
        _myCharacterData.InitializeRoutineRecordDictionary();
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
}
