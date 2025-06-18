using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class SoundManager : SingletonBase<SoundManager>, IManager  
{
    [SerializeField] private SoundData _soundData;
    [SerializeField] private AudioSource musicPlayer;
    [SerializeField] private AudioClip thirtyMinutesSound;
    [SerializeField] private AudioClip alarmSound;

    public void Init()
    {
        Debug.Log("SoundManager");
    }
    
    // TODO : Erase - 여기 들어가기에는 너무 specific
    public IEnumerator Play()
    {
        Debug.Log("Play");
        musicPlayer.volume = 0.5f;
        musicPlayer.loop = false;
        musicPlayer.PlayOneShot(thirtyMinutesSound);

        //yield return new WaitForSeconds(PLAY_TIME);

        Debug.Log("Stop");
        musicPlayer.Stop();

        musicPlayer.volume = 1f;
        musicPlayer.clip = alarmSound;
        musicPlayer.loop = true;
        yield return new WaitForSeconds(0.5f);
        musicPlayer.Play();
    }

    public bool IsMusicPlaying()
    {
        return musicPlayer.isPlaying;
    }
}
