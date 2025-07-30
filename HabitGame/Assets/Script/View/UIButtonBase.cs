using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonBase : MonoBehaviour, IView
{
    #region 1. Fields

    [SerializeField] private Canvas _canvas;
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _buttonText;
    [SerializeField] private EStringKey _buttonTextKey;

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
        _buttonPresenter = SoundManager.Instance.GetPresenterAfterCreate<ButtonPresenterBase>(this);

        SetButtonText();
        
        BindEvent();
    }

    public void BindEvent()
    {
        BindEventInternal();
    }
    
    // todo
    // 버그 발생 우려가 있긴 함.
    protected virtual void BindEventInternal()
    {
        _button.onClick.AddListener(() => _onClickButton.OnNext(default));
    }

    private void SetButtonText()
    {
        ExceptionHelper.CheckNullException(_buttonText, "buttonText");

        _buttonText.text = StringManager.Instance.GetUIString(_buttonTextKey);
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