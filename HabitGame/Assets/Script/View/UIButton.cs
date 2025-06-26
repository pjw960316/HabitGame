using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour, IView
{
    [SerializeField] protected Button button;
    [SerializeField] private Text buttonText;

    private IPresenter _presenter;

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

    private void SetPresenter(IPresenter presenter)
    {
        
    }
}