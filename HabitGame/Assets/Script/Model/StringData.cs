using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "StringData", menuName = "ScriptableObjects/StringData")]
public class StringData : ScriptableObject, IModel
{
    // TODO
    // 분해를 많이 하자
    #region 1. Fields

    [SerializeField] private SerializedDictionary<EStringKey, string> _stringDictionary = new();

    #endregion

    #region 2. Properties

    public SerializedDictionary<EStringKey, string> StringDictionary => _stringDictionary;

    #endregion

    #region 3. Constructor

    public void Initialize()
    {
    }

    #endregion

    #region 4. Methods

    // refactor
    // 예외처리
    public string GetStringByEStringKey(EStringKey eStringKey)
    {
        return _stringDictionary[eStringKey];
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

public enum EStringKey
{
    EAlarmButton,
    ERoutineCheckButton,
    ERoutineRecordButton,
    EAlarmPopupAlarmMusicOne,
    EAlarmPopupAlarmMusicTwo,
    EAlarmPopupAlarmMusicThree,
    EAlarmPopupAlarmTimeOne,
    EAlarmPopupAlarmTimeTwo,
    EAlarmPopupAlarmTimeThree,
}