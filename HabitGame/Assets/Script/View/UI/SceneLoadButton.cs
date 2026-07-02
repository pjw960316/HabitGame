using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public sealed class SceneLoadButton : MonoBehaviour
{
    [SerializeField] private string _sceneName = "BlankScene";

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(LoadScene);
    }

    private void OnDestroy()
    {
        if (_button == null)
        {
            return;
        }

        _button.onClick.RemoveListener(LoadScene);
    }

    private void LoadScene()
    {
        SceneChangeManager.Instance.LoadScene(_sceneName);
    }
}
