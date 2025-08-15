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

    public List<int> GetTodaySuccessfulRoutineIndex(DateTime dateTime)
    {
        var key = dateTime.ToString("yyyyMMdd");
        var routineRecordDictionary = _myCharacterData.RoutineRecordDictionary;

        if (routineRecordDictionary.ContainsKey(key) == false)
        {
            routineRecordDictionary.Add(key, new List<bool>
            {
                false,
                false,
                false,
                false
            });
        }

        var todayRecordList = routineRecordDictionary[key];
        var todaySuccessfulRoutineIndex = new List<int>();

        for (var index = 0; index < todayRecordList.Count; index++)
        {
            var todayRecord = todayRecordList[index];
            if (todayRecord)
            {
                todaySuccessfulRoutineIndex.Add(index);
            }
        }

        return todaySuccessfulRoutineIndex;
    }

    public void UpdateRoutineRecord(List<int> todaySuccessfulRoutineIndex, DateTime dateTime)
    {
        UpdateCurrentRoutineRecordData(todaySuccessfulRoutineIndex, dateTime);

        RequestUpdateXmlData();
    }

    // note
    // 유저가 오늘 완료 여부를 체크한 루틴의 index를 리스트로 받는다. (index는 0부터)
    private void UpdateCurrentRoutineRecordData(List<int> todaySuccessfulRoutineIndexByView, DateTime dateTime)
    {
        var key = dateTime.ToString("yyyyMMdd");

        var routineRecordDictionary = _myCharacterData.RoutineRecordDictionary;

        // note
        // 오늘의 루틴 완료 여부를 0번 루틴 ~ 마지막 루틴 까지 bool로 저장
        var todayRoutineRecordList = routineRecordDictionary[key];

        var reward = 0;
        foreach (var index in todaySuccessfulRoutineIndexByView)
        {
            // note
            // 기존에 false 였는데 유저가 체크했다면 얘는 최근에 수행했다는 뜻.
            if (todayRoutineRecordList[index] == false)
            {
                todayRoutineRecordList[index] = true;
                reward += _myCharacterData.MoneyPerRoutineSuccess;
            }
        }

        UpdateMonthlyRoutineSuccessMoney(reward);
    }

    private void UpdateMonthlyRoutineSuccessMoney(int reward)
    {
        _myCharacterData.MonthlyRoutineSuccessMoney += reward;
    }

    public int GetMonthlyRoutineSuccessMoney()
    {
        return _myCharacterData.MonthlyRoutineSuccessMoney;
    }

    #endregion

    #region 5. Request Methods

    private void RequestUpdateXmlData()
    {
        _xmlDataSerializeManager.SerializeXmlData<MyCharacterData>(_myCharacterData);
    }

    #endregion

    #region 6. EventHandlers

    // default

    #endregion
}