using System.Collections.Immutable;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "AlarmData", menuName = "ScriptableObjects/AlarmData")]
public class AlarmData : ScriptableObject, IModel
{
    #region 1. Fields

    [SerializeField] private SerializedDictionary<EAlarmButtonType, AudioClip> _alarmAudioClipDictionary = new();
    [SerializeField] private SerializedDictionary<EAlarmButtonType, float> _alarmTimeDictionary = new();
    [SerializeField] private AudioClip _alarmChickenAudioClip;

    #endregion

    #region 2. Properties

    public AudioClip AlarmChickenAudioClip => _alarmChickenAudioClip;
    public ImmutableDictionary<EAlarmButtonType, float> AlarmTimeDictionary => _alarmTimeDictionary.ToImmutableDictionary();

    #endregion

    #region 3. Constructor

    //

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    public AudioClip GetAlarmAudioClip(EAlarmButtonType eAlarmAudioClip)
    {
        _alarmAudioClipDictionary.TryGetValue(eAlarmAudioClip, out var audioClip);
        return audioClip;
    }

    public float GetAlarmTime(EAlarmButtonType eAlarmTime)
    {
        _alarmTimeDictionary.TryGetValue(eAlarmTime, out var alarmTime);
        return alarmTime;
    }

    public AudioClip GetDefaultAlarmAudioClip()
    {
        return _alarmAudioClipDictionary.FirstOrDefault().Value;
    }

    public float GetDefaultAlarmTime()
    {
        return _alarmTimeDictionary.FirstOrDefault().Value;
    }

    #endregion
}

public enum EAlarmButtonType
{
    MusicOne,
    MusicTwo,
    MusicThree,
    DivisionConst,
    TimeOne,
    TimeTwo,
    TimeThree,
    TimeFour,
    TimeFive
}