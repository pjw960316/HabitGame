using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonBase : MonoBehaviour, IView
{
    #region 1. Fields

    [SerializeField] protected Button Button;
    [SerializeField] protected TextMeshProUGUI ButtonText;

    private readonly Subject<Unit> _onClickButton = new();
    public IObservable<Unit> OnClickButton => _onClickButton;

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    protected virtual void Awake()
    {
        //test
        Debug.Log("Awake UIButtonBase");
        
        Initialize();
    }

    #endregion

    #region 4. Methods

    private void Initialize()
    {
        BindEvent();
    }

    protected virtual void BindEvent()
    {
        Button.onClick.AddListener(() => _onClickButton.OnNext(default));
    }

    #endregion

    #region 5. EventHandlers

    //default

    #endregion
}