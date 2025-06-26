using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    [SerializeField] protected Button button;
    [SerializeField] private Text buttonText;


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
}