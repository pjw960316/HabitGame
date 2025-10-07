using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "AlarmData", menuName = "ScriptableObjects/AlarmData")]
public class AlarmData : ScriptableObject, IModel
{
    #region 1. Fields

    private const string BaseDirectoryName = "Music";

    // note
    // AudioClip의 메모리 적재를 GameStartManagerMono로 옮긴다.
    // 그러기 위해 Path가 필요하다.
    // Path를 추출하기 위한 용도고, 실제로 메모리에 적재되지 않는다.
    private readonly Dictionary<EAlarmButtonType, string> _sleepingAudioClipPathDictionary = new();
    
    [SerializeField] private SerializedDictionary<EAlarmButtonType, AudioClip> _sleepingAudioClipDictionary = new();
    [SerializeField] private SerializedDictionary<EAlarmButtonType, float> _sleepingAudioPlayTimeDictionary = new();
    [SerializeField] private AudioClip _alarmAudioClip;

    #endregion

    #region 2. Properties

    public AudioClip AlarmAudioClip => _alarmAudioClip;

    public AudioClip LatestSleepingAudioClip { get; private set; }

    public float LatestSleepingAudioPlayTime { get; private set; }

    public ImmutableDictionary<EAlarmButtonType, string> SleepingAudioClipPathDictionary =>
        _sleepingAudioClipPathDictionary.ToImmutableDictionary();

    public ImmutableDictionary<EAlarmButtonType, float> SleepingAudioPlayTimeDictionary =>
        _sleepingAudioPlayTimeDictionary.ToImmutableDictionary();

    #endregion

    #region 3. Constructor

    public void Initialize()
    {
        InitializeSleepingAudioClipPathDictionary();
        
        InitializeAudioClipAndPlayTimeToDefault();
    }

    private void InitializeSleepingAudioClipPathDictionary()
    {
        foreach (var audioClip in _sleepingAudioClipDictionary)
        {
            var audioClipName = audioClip.Value.name;
            var path = $"{BaseDirectoryName}/{audioClipName}";

            _sleepingAudioClipPathDictionary[audioClip.Key] = path;
        }
    }

    private void InitializeAudioClipAndPlayTimeToDefault()
    {
        LatestSleepingAudioClip = _sleepingAudioClipDictionary.FirstOrDefault().Value;
        LatestSleepingAudioPlayTime = 0f;
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

    public void SetSleepingAudioClipDictionary(EAlarmButtonType eAlarmButtonType, AudioClip memoryLoadedAudioClip)
    {
        _sleepingAudioClipDictionary[eAlarmButtonType] = memoryLoadedAudioClip;
    }

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

    public void RestoreAudioClipAndPlayTimeToDefault()
    {
        InitializeAudioClipAndPlayTimeToDefault();
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