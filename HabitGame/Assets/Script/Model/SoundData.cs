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

    //[SerializeField] private AudioClip _airplaneSound;
    //public AudioClip AirplaneSound => _airplaneSound;

    #endregion

    #region 3. Constructor

    public SoundData()
    {
        Debug.Log("Sound Data Created");
        var a = AlarmSound;
        Debug.Log(a.length);

    }
    // default

    #endregion

    #region 4. Methods

    // default

    #endregion
}