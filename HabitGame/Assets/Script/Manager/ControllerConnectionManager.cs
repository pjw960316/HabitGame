public sealed class ControllerConnectionManager : ManagerBase<ControllerConnectionManager>
{
    #region 1. Fields

    //

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    //

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    public void ConnectManager<TManager, TController>(TController controller)
        where TManager : class, IManager, IControllerRegister<TController>
        where TController : ControllerBase
    {
        var manager = GameManager.Instance.GetManagerByType<TManager>();
        manager.RegisterController(controller);
    }

    #endregion

    #region 6. Methods

    //

    #endregion
}
