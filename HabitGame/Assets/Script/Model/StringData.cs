using System;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "StringData", menuName = "ScriptableObjects/StringData")]
public class StringData : ScriptableObject, IModel
{
    #region 1. Fields

    [SerializeField] private SerializedDictionary<EStringKey, string> _stringDictionary = new();
    [SerializeField] private SerializedDictionary<EToastStringKey, string> _toastStringDictionary = new();

    #endregion

    #region 2. Properties

    public SerializedDictionary<EStringKey, string> StringDictionary => _stringDictionary;
    public SerializedDictionary<EToastStringKey, string> ToastStringDictionary => _toastStringDictionary;

    #endregion

    #region 3. Constructor

    public void Initialize()
    {
    }

    #endregion

    #region 4. Methods

    public string GetStringByEStringKey(EStringKey eStringKey)
    {
        if (_stringDictionary.ContainsKey(eStringKey) == false)
        {
            throw new ArgumentException("StringDictionary 에서 이용할 Key가 올바르지 않습니다.");
        }

        return _stringDictionary[eStringKey];
    }

    public string GetStringByEStringKeyAndArguments(EStringKey eStringKey, params object[] args)
    {
        if (_stringDictionary.ContainsKey(eStringKey) == false)
        {
            throw new ArgumentException("StringDictionary 에서 이용할 Key가 올바르지 않습니다.");
        }
        
        var str = _stringDictionary[eStringKey];
        
        return String.Format(str, args);
    }

    public string GetToastString(EToastStringKey eToastStringKey)
    {
        if (_toastStringDictionary.ContainsKey(eToastStringKey) == false)
        {
            throw new ArgumentException("ToastMessage Dictionary 에서 이용할 Key가 올바르지 않습니다.");
        }

        return _toastStringDictionary[eToastStringKey];
    }

    private void ParseString()
    {
    }

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}

public interface IEnumKey
{
}

// note
// 아래부터 추가하세요
public enum EStringKey
{
    EConfirmButton,
    EAlarmButton,
    ERoutineCheckButton,
    ERoutineRecordButton,
    EAlarmPopupAlarmMusicOne,
    EAlarmPopupAlarmMusicTwo,
    EAlarmPopupAlarmMusicThree,
    EAlarmPopupAlarmTime,
    ERoutineCheckOne,
    ERoutineCheckTwo,
    ERoutineCheckThree,
    ERoutineCheckFour,
    ERoutineRecordTitle,
    EAlarmTimerPopupTime,
    EQuitAlarmButton,
    EAlarmTimerPopupTitle,
}

// note
// 아래부터 추가하세요
public enum EToastStringKey
{
    EAlarmConfirm,
    ERoutineCheckConfirm,
    EAlarmQuit,
    EAlarmSelectPlease,
}