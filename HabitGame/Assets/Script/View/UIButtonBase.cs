using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonBase : MonoBehaviour, IView
{
    #region 1. Fields

    [SerializeField] private Canvas _canvas;
    [SerializeField] protected Button Button;
    [SerializeField] protected TextMeshProUGUI ButtonText;

    private ButtonPresenterBase _buttonPresenter;

    private readonly Subject<Unit> _onClickButton = new();
    public IObservable<Unit> OnClickButton => _onClickButton;

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    protected virtual void Awake()
    {
        Initialize();
    }

    #endregion

    #region 4. Methods

    private void Initialize()
    {
        _buttonPresenter = SoundManager.Instance.GetPresenterAfterCreate(this) as ButtonPresenterBase;
        
        BindEvent();
    }

    //refactor
    //virtual?
    private void BindEvent()
    {
        Button.onClick.AddListener(() => _onClickButton.OnNext(default));
    }

    public Canvas GetCanvas()
    {
        return _canvas;
    }

    #endregion

    #region 5. EventHandlers

    //default

    #endregion
}