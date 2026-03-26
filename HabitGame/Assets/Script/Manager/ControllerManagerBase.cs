public abstract class ControllerManagerBase<TManager, TController>
    : ManagerBase<TManager>
    where TManager : class, new()
    where TController : IController
{
    protected TController _controller;

    public void RegisterController(TController controller)
    {
        _controller = controller;
    }
}