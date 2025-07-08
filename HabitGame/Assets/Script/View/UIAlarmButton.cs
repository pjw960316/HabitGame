public class UIAlarmButton : UIButtonBase
{
    #region 1. Fields

    private IPresenter _alarmPresenter;

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    protected override void Awake()
    {
        base.Awake();
        
        _alarmPresenter = SoundManager.Instance.GetPresenterAfterCreate(this);
    }

    #endregion

    #region 4. Methods

   //default

    #endregion

    #region 5. EventHandlers

//default
    #endregion
}