public interface IControllerRegister<in TController>
    where TController : ControllerBase
{
    public void RegisterController(TController controller);
}
