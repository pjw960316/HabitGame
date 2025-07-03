using UniRx;
using UnityEngine;

public class AlarmPresenter : IPresenter
{
    #region 1. Fields
    //test
    private UIAlarmPopup _view;
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
        _view = view as UIAlarmPopup;

        //_view = view;
        _model = model;

        BindEvent();
    }
    #endregion

    #region 4. Methods

    private void BindEvent()
    {
        _view.OnSoundButtonClicked.Subscribe(unit => OpenPopup()).AddTo(_disposable);
    }
    
    #endregion
    
    #region 5. EventHandlers

    private void OpenPopup()
    {
        Debug.Log("test suce");
    }
    #endregion
    
    
    
    
    
    
    
}