public class AlarmPresenter : IPresenter
{
    private IView _view;
    private readonly IModel _model;

    public AlarmPresenter(IView view, IModel model)
    {
        _view = view;
        _model = model;
    }
}