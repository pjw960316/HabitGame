using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIButton : MonoBehaviour, IView
{
    [SerializeField] protected Button button;
    [SerializeField] protected TextMeshProUGUI buttonText;

    protected virtual void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        BindEvent();
    }

    protected virtual void BindEvent()
    {
    }

    public abstract void HoldPresenterInterface();
}