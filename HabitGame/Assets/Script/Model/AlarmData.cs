using System.Collections.Generic;
using System.Collections.Immutable;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "AlarmData", menuName = "ScriptableObjects/AlarmData")]
public class AlarmData : ScriptableObject, IModel
{
    #region 1. Fields
    
    [SerializeField] private SerializedDictionary<EAlarmButtonType, float> _sleepingAudioPlayTimeDictionary = new();
    [SerializeField] private AudioClip _alarmAudioClip;
    private readonly Dictionary<EAlarmButtonType, AudioClip> _sleepingAudioClipDictionary = new();
    
    #endregion

    #region 2. Properties

    public AudioClip AlarmAudioClip => _alarmAudioClip;

    public AudioClip LatestSleepingAudioClip { get; private set; }

    public float LatestSleepingAudioPlayTime { get; private set; }

    public ImmutableDictionary<EAlarmButtonType, float> SleepingAudioPlayTimeDictionary =>
        _sleepingAudioPlayTimeDictionary.ToImmutableDictionary();

    #endregion

    #region 3. Constructor

    //test
    public void Initialize(List<AudioClip> TestAudioClip)
    {
        foreach (var i in TestAudioClip)
        {
            if (i.name == "30Minutes_Jambaksa")
            {
                _sleepingAudioClipDictionary[EAlarmButtonType.MusicOne] = i;
                Debug.Log("1");
            }

            if (i.name == "Airplane")
            {
                _sleepingAudioClipDictionary[EAlarmButtonType.MusicTwo] = i;
                Debug.Log("2");
            }
        }
    }

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

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