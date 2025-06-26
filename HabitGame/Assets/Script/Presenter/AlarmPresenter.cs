using UnityEngine;

public class AlarmPresenter : IPresenter
{
    private IView _view;
    private IModel _model;

    public AlarmPresenter()
    {
        
    }

    public void InjectView(IView view)
    {
        _view = view;
    }
}