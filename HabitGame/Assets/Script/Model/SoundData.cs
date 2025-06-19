using UnityEngine;

[CreateAssetMenu(fileName = "SoundData", menuName = "ScriptableObjects/SoundData", order = 1)]
public class SoundData : ScriptableObject
{
    #region 1. Fields
    // default
    #endregion
    
    #region 2. Properties
    
    [SerializeField] private AudioClip _alarmSound;
    public AudioClip AlarmSound => _alarmSound;
    
    [SerializeField] private AudioClip _airplaneSound;
    public AudioClip AirplaneSound => _airplaneSound;
    
    #endregion
    
    #region 3. Constructor
    // default
    #endregion

    #region 4. Methods
    // 데이터를 가공하는 정도?
    

    #endregion
}
