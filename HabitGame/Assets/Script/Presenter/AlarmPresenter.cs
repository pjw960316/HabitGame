using UniRx;
using UnityEngine;

public class AlarmPresenter : IPresenter
{
    #region 1. Fields
    //test
    private UIAlarmButton _view;
    //private IView _view;
    private readonly IModel _model;
    
    //test
    
    private CompositeDisposable _disposable = new();
    #endregion
    
    #region 2. Properties
    // default
    #endregion
    
    #region 3. Constructor
    public AlarmPresenter(IView view, IModel model)
    {
        //test
        _view = view as UIAlarmButton;

        //_view = view;
        _model = model;

        BindEvent();
    }
    #endregion

    #region 4. Methods

    private void BindEvent()
    {
        _view.TestEventAsObservable.Subscribe(unit => OpenPopup()).AddTo(_disposable);
    }
    
    #endregion
    
    #region 5. EventHandlers

    private void OpenPopup()
    {
        Debug.Log("open popup");
    }
    #endregion
    
    
    
    
    
    
    
}