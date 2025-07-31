public class UIOpenPopupButtonBase : UIButtonBase
{
    #region 1. Fields

    //

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    public override void OnAwake()
    {
        base.OnAwake();

        CreatePresenterByManager();
        BindEvent();
    }

    #endregion

    #region 4. Methods

    private void BindEvent()
    {
        _button.onClick.AddListener(() => _onClickButton.OnNext(default));
    }

    // todo : T를 뭐로 해야할까?
    protected override void CreatePresenterByManager()
    {
        _uiManager.CreatePresenter<ButtonPresenterBase>(this);
    }

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}