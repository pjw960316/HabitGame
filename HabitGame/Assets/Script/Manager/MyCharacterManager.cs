using System;
using System.Collections.Generic;

public class MyCharacterManager : ManagerBase<MyCharacterManager>, IManager
{
    #region 1. Fields

    // Note
    // Model을 외부에서 접근하게 하지말고
    // 필요하면 argument로 전달해라
    private MyCharacterData _myCharacterData;
    private XmlDataSerializeManager _xmlDataSerializeManager;

    //

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    public void PreInitialize()
    {
        //
    }

    public void Initialize()
    {
        _xmlDataSerializeManager = XmlDataSerializeManager.Instance;

        ExceptionHelper.CheckNullExceptionWithMessage(_xmlDataSerializeManager, "xmlDataSerializeManager",
            "\n GameStartManager Initialize() makes critical Error!");
    }

    #endregion

    #region 4. Methods

    public void SetModel(IEnumerable<IModel> models)
    {
        foreach (var model in models)
        {
            if (model is MyCharacterData myCharacterData)
            {
                _myCharacterData = myCharacterData;
                break;
            }
        }

        ExceptionHelper.CheckNullException(_myCharacterData, "_myCharacterData in MyCharacterManager");
    }

    //todo 예외처리
    public List<int> GetTodayCompletedRoutineIndex(DateTime dateTime)
    {
        var key = dateTime.ToString("yyyyMMdd");
        
        //var routineRecordDictionary = _myCharacterData.RoutineRecordDictionary;
        //var todayRecordList = routineRecordDictionary[key];
        
        
        //test
        var todayRecordList = new List<bool>
        {
            true,
            false,
            false,
            true
        };
        //여기까지

        var completedRoutineIndexList = new List<int>();
        
        for (var index = 0; index < todayRecordList.Count; index++)
        {
            var todayRecord = todayRecordList[index];
            
            if (todayRecord == true)
            {
                completedRoutineIndexList.Add(index);
            }
        }

        return completedRoutineIndexList;
    }

    public void UpdateRoutineRecord(List<int> todayCompletedRoutineIndexList, DateTime dateTime)
    {
        UpdateCurrentRoutineRecordData(todayCompletedRoutineIndexList, dateTime);
        
        RequestUpdateXmlData();
    }

    // note
    // 유저가 오늘 완료 여부를 체크한 루틴의 index를 리스트로 받는다. (index는 0부터)
    private void UpdateCurrentRoutineRecordData(List<int> todayCompletedRoutineIndexListByUser, DateTime dateTime)
    {
        var key = dateTime.ToString("yyyyMMdd");
        
        var routineRecordDictionary = _myCharacterData.RoutineRecordDictionary;
        
        // note
        // 오늘의 루틴 완료 여부를 0번 루틴 ~ 마지막 루틴 까지 bool로 저장
        var todayRoutineRecordList = routineRecordDictionary[key];

        var reward = 0;
        foreach (var index in todayCompletedRoutineIndexListByUser)
        {
            // note
            // 기존에 false 였는데 유저가 체크했다면 얘는 최근에 수행했다는 뜻.
            if (todayRoutineRecordList[index] == false)
            {
                // refactor
                // 여기서 데이터를 바꾸면 MyCharacterData의 property set에 안 잡히는 데 이거 연구
                todayRoutineRecordList[index] = true;
                reward += _myCharacterData.RewardPerRoutineSuccess;
            }
        }

        UpdateRoutineSuccessRewardMoney(reward);
    }

    private void UpdateRoutineSuccessRewardMoney(int reward)
    {
        _myCharacterData.CurrentRoutineSuccessRewardMoney += reward;
    }

    private void RequestUpdateXmlData()
    {
        _xmlDataSerializeManager.SerializeXmlData<MyCharacterData>(_myCharacterData);
    }
    
    public int GetCurrentRoutineSuccessRewardMoney()
    {
        return _myCharacterData.CurrentRoutineSuccessRewardMoney;
    }

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}