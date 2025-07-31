using System;
using UniRx;

public class UIOpenPopupButtonBase : UIButtonBase
{
    #region 1. Fields

    private readonly Subject<EPopupKey> _onClickButton = new();
    public IObservable<EPopupKey> OnClickButton => _onClickButton;

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    public override void OnAwake()
    {
        base.OnAwake();

        BindEvent();
    }

    #endregion

    #region 4. Methods

    private void BindEvent()
    {
        Button.onClick.AddListener(() => _onClickButton.OnNext(default));
    }

    protected override void ConnectPresenter()
    {
        
    }

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}