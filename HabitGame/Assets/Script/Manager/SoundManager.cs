using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class SoundManager : SingletonBase<SoundManager>, IManager  
{
    [SerializeField] private SoundData _soundData;
    /*public SoundManager()
    {
        
    }*/
    // 싱글턴 인스턴스를 담는 정적 변수
    private static SoundManager _soundManager;

    [SerializeField] private AudioSource musicPlayer;

    [SerializeField] private AudioClip thirtyMinutesSound;

    [SerializeField] private AudioClip alarmSound;

    private const float PLAY_TIME = 30*60;

    // 외부에서 접근 가능한 프로퍼티

    public void Init()
    {
        Debug.Log("SoundManager");
    }
    public IEnumerator Play()
    {
        Debug.Log("Play");
        musicPlayer.volume = 0.5f;
        musicPlayer.loop = false;
        musicPlayer.PlayOneShot(thirtyMinutesSound);

        yield return new WaitForSeconds(PLAY_TIME);

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
