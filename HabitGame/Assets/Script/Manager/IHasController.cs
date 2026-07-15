public interface IHasController<in TController>
    where TController : ControllerBase
{
    public void RegisterController(TController controller);
}
