using UnityEngine;
using UnityEngine.UI;

public class UIAlarmButton : MonoBehaviour
{
    [SerializeField] private Button button;

    private void Awake()
    {
        if (button != null)
        {
            button.onClick.AddListener(OnClicked);
        }
    }


    private void OnClicked()
    {
        var soundManager = SoundManager.Instance;

        if (soundManager.IsMusicPlaying())
        {
            return;
        }

        StartCoroutine(soundManager.Play());
    }
}
