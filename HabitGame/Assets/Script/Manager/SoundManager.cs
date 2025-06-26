using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class SoundManager : SingletonBase<SoundManager>, IManager
{
    #region 1. Fields

    // default

    #endregion

    #region 2. Properties

    private SoundData _soundData;
    [SerializeField] private AudioSource musicPlayer;

    #endregion

    #region 3. Constructor

    // default

    #endregion

    #region 4. Methods

    public void Init()
    {
    }
    
    public void InitializeScriptableObject(IData data)
    {
        if (data is SoundData soundData)
        {
            _soundData = soundData;
        }
        
        //TODO : test Code
        if (_soundData != null)
        {
            //Debug.Log("today end");
        }
    }

    // TODO : Erase - 여기 들어가기에는 너무 specific
    public IEnumerator Play()
    {
        Debug.Log("Play");
        musicPlayer.volume = 0.5f;
        musicPlayer.loop = false;
        //musicPlayer.PlayOneShot(thirtyMinutesSound);

        //yield return new WaitForSeconds(PLAY_TIME);

        Debug.Log("Stop");
        musicPlayer.Stop();

        musicPlayer.volume = 1f;
        //musicPlayer.clip = alarmSound;
        musicPlayer.loop = true;
        yield return new WaitForSeconds(0.5f);
        musicPlayer.Play();
    }

    public bool IsMusicPlaying()
    {
        return musicPlayer.isPlaying;
    }

    private void Test()
    {
    }

    #endregion
}