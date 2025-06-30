using UnityEngine;

public class AlarmPresenter : IPresenter
{
    private IView _view;
    private IModel _model;

    public AlarmPresenter(IView view, IModel model)
    {
        _view = view;
        _model = model;
    }

    public void InjectView(IView view)
    {
        _view = view;
    }

    private void InjectModel(IModel model)
    {
        _model = model;
    }
}