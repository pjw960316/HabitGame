using System.Collections.Immutable;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "AlarmData", menuName = "ScriptableObjects/AlarmData")]
public class AlarmData : ScriptableObject, IModel
{
    #region 1. Fields

    // note : 수면 음원
    [SerializeField] private SerializedDictionary<EAlarmButtonType, AudioClip> _sleepingAudioClipDictionary = new();
    [SerializeField] private SerializedDictionary<EAlarmButtonType, float> _sleepingAudioPlayTimeDictionary = new();
    
    // note : 알람 음원
    [SerializeField] private AudioClip _alarmAudioClip;

    #endregion

    #region 2. Properties

    public AudioClip AlarmAudioClip => _alarmAudioClip;

    public AudioClip LatestSleepingAudioClip { get; private set; }

    public float LatestSleepingAudioPlayTime { get; private set; }


    public ImmutableDictionary<EAlarmButtonType, float> SleepingAudioPlayTimeDictionary =>
        _sleepingAudioPlayTimeDictionary.ToImmutableDictionary();

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

    // note : getter
    public AudioClip GetSleepingAudioClip(EAlarmButtonType eAlarmAudioClip)
    {
        _sleepingAudioClipDictionary.TryGetValue(eAlarmAudioClip, out var audioClip);
        return audioClip;
    }

    public float GetSleepingAudioPlayTime(EAlarmButtonType eAlarmTime)
    {
        _sleepingAudioPlayTimeDictionary.TryGetValue(eAlarmTime, out var alarmTime);
        return alarmTime;
    }

    public AudioClip GetDefaultSleepingAudioClip()
    {
        return _sleepingAudioClipDictionary.FirstOrDefault().Value;
    }

    public float GetDefaultSleepingAudioPlayTime()
    {
        return _sleepingAudioPlayTimeDictionary.FirstOrDefault().Value;
    }

    // note : setter
    public void SetLatestSleepingAudioClip(EAlarmButtonType eAlarmAudioClip)
    {
        _sleepingAudioClipDictionary.TryGetValue(eAlarmAudioClip, out var value);
        LatestSleepingAudioClip = value;
    }

    public void SetLatestSleepingAudioPlayTime(EAlarmButtonType eAlarmAudioClip)
    {
        _sleepingAudioPlayTimeDictionary.TryGetValue(eAlarmAudioClip, out var value);
        LatestSleepingAudioPlayTime = value;
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