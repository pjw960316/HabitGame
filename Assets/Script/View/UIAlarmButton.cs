using UnityEngine;
using UnityEngine.UI;

public class UIAlarmButton : MonoBehaviour
{
    [SerializeField] private Button button;

    private void Awake()
    {
        // TODO : Binding 여기서 X
        if (button)
        {
            button.onClick.AddListener(OnClicked);
        }
    }


    private void OnClicked()
    {
        // TODO : soundManager를 너는 몰라야 해
        var soundManager = SoundManager.Instance;

        if (soundManager.IsMusicPlaying())
        {
            return;
        }

        StartCoroutine(soundManager.Play());
    }
}
