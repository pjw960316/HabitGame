// NOTE
// 책임은 단순하다.
// Controller가 생성되면 그에 걸맞는 Manager를 연결시켜준다. Manager가 그 Controller를 들고 있는다.
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

    #region 5. Methods
    //

    public void ConnectManager<TManager, TController>(TController controller)
        where TManager : class, IManager, IHasController<TController>
        where TController : ControllerBase
    {
        var targetManager = GameManager.Instance.GetManagerByType<TManager>();

        targetManager.RegisterController(controller);
    }

    #endregion
}
