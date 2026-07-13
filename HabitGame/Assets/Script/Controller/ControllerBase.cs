using UnityEngine;

// NOTE
// 1. 모든 Controller는 Manager보다 늦게 생성된다.
// 2. 모든 Controller는 자신과 1대1로 연결되는 Manager의 타입을 알고 있다.
// 3. 모든 Controller는 자신과 연결해야 할 Manager 타입을 Generic으로 외부에 요청한다.
public abstract class ControllerBase : MonoBehaviour, IController
{
    #region 1. Fields

    //

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    private void Awake()
    {
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

    protected void RequestConnectManager<TManager, TController>(TController controller)
        where TManager : class, IManager, IControllerRegister<TController>
        where TController : ControllerBase
    {
        ControllerConnectionManager.Instance.ConnectManager<TManager, TController>(controller);
    }

    #endregion

    #region 6. Methods

    //

    #endregion
}
