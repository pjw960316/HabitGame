using UnityEngine;

public class UIOpenPopupButtonBase : UIButtonBase
{
    #region 1. Fields

    [SerializeField] private EPopupKey ePopupKey;

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
        _button.onClick.AddListener(() => _onClickButton.OnNext(ePopupKey));
    }

    public void CreatePresenterByManager()
    {
        _uiManager.CreatePresenter<OpenPopupPresenterBase>(this);
    }

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}