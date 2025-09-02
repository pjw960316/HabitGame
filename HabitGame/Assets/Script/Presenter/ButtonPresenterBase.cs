using System;

// note
// 추후에 필요하면 사용하자.
public class ButtonPresenterBase : PresenterBase
{
    #region 1. Fields

    protected UIButtonBase _uiButtonBase;
    protected UIManager _uiManager;

    #endregion

    #region 2. Properties

    // 

    #endregion

    #region 3. Constructor

    public override void Initialize(IView view)
    {
        base.Initialize(view);
        
        if (view is UIButtonBase uiButton)
        {
            _uiButtonBase = uiButton;
        }
        else
        {
            throw new NullReferenceException("_ButtonPresenterBase should connect UIButtonBase & it's children");
        }

        _uiManager = UIManager.Instance;

        SetView();

        BindEvent();
    }

    protected sealed override void SetView()
    {
    }

    private void BindEvent()
    {
    }

    #endregion

    #region 4. EventHandlers

    // 

    #endregion

    #region 5. Request Methods

    //

    #endregion

    #region 6. Methods

    //

    #endregion
}