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

    // TODO
    // 네이밍 변경
    private IPresenter _presenter;

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

        //test
        (_presenter as ButtonPresenterBase).SetCanvas(_canvas);
    }

    #endregion

    #region 4. Methods

    private void Initialize()
    {
        _presenter = SoundManager.Instance.GetPresenterAfterCreate(this);
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