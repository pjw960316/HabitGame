using UnityEngine;

public abstract class ControllerBase : MonoBehaviour, IController
{
    #region 1. Fields

    private ControllerConnectionManager _controllerConnectionManager;
    #endregion

    #region 2. Properties
    //
    #endregion

    #region 3. Constructor

    protected virtual void Awake()
    {
        _controllerConnectionManager = ControllerConnectionManager.Instance;
        
        Initialize();
    }

    protected virtual void Initialize()
    {
    }

    #endregion

    #region 4. EventHandlers
    //
    #endregion

    #region 5. Request Methods

    // WARNING
    // 하위 타입에서 항상 호출시키세요.
    // ControllerBase를 비제네릭 클래스로 유지하고 싶다.
    // 그러므로, 해당 메서드를 하위 타입에서 강제 할 수 없다.
    protected void RequestConnectManager<TManager, TController>(TController controller)
        where TManager : class, IManager, IHasController<TController>
        where TController : ControllerBase
    {
        _controllerConnectionManager.ConnectManager<TManager, TController>(controller);
    }

    #endregion

    #region 6. Methods
    //
    #endregion
}
