using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

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

        //test
        var routineRecordList = _myCharacterData.RoutineRecordList;
        foreach (var routineRecordData in routineRecordList)
        {
            Debug.Log("key" + $"{routineRecordData.key}");
            Debug.Log("0" + $"{routineRecordData.RoutineCheckList[0]}");
            Debug.Log("1" + $"{routineRecordData.RoutineCheckList[1]}");
            Debug.Log("2" + $"{routineRecordData.RoutineCheckList[2]}");
            Debug.Log("3" + $"{routineRecordData.RoutineCheckList[3]}");
        }

        RequestInitializeRoutineRecordDictionary();
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

    [CanBeNull]
    public List<int> GetTodaySuccessfulRoutineIndex(DateTime dateTime)
    {
        var key = dateTime.ToString("yyyyMMdd");
        var immutableRoutineRecordDictionary = _myCharacterData.RoutineRecordDictionary;

        if (immutableRoutineRecordDictionary.TryGetValue(key, out var immutableTodayRecordList) == false)
        {
            // note
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

    public void UpdateRoutineRecord(List<int> todaySuccessfulRoutineIndexByView, DateTime dateTime)
    {
        RequestUpdateRoutineRecordDictionary(todaySuccessfulRoutineIndexByView, dateTime);

        //refactor
        //이걸 매번 할 필요는 없지?
        RequestUpdateXmlData();
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

    private void RequestInitializeRoutineRecordDictionary()
    {
        _myCharacterData.InitializeRoutineRecordDictionary();
    }

    private void RequestUpdateRoutineRecordDictionary(List<int> todaySuccessfulRoutineIndexByView, DateTime dateTime)
    {
        _myCharacterData.UpdateRoutineRecordDictionary(todaySuccessfulRoutineIndexByView, dateTime);
    }

    #endregion

    #region 6. EventHandlers

    // default

    #endregion
}