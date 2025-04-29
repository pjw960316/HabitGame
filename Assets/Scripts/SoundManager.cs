using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    // 싱글턴 인스턴스를 담는 정적 변수
    private static SoundManager _soundManager;

    [SerializeField] private AudioSource musicPlayer;

    [SerializeField] private AudioClip thirtyMinutesSound;

    [SerializeField] private AudioClip alarmSound;

    private const float PLAY_TIME = 30*60;

    // 외부에서 접근 가능한 프로퍼티
    public static SoundManager Instance
    {
        get
        {
            // 인스턴스가 없으면 새로 생성 (옵션)
            if (_soundManager == null)
            {
                _soundManager = FindObjectOfType<SoundManager>(); //이 방식 X
            }

            return _soundManager;
        }
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
