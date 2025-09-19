using System;
using UniRx;

public abstract class FieldObjectPresenterBase : IPresenter
{
    #region 1. Fields

    protected PresenterManager _presenterManager;
    
    protected FieldObjectBase _view;
    protected IModel _model;
    protected readonly CompositeDisposable _disposable = new();

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    public virtual void Initialize(IView view)
    {
        _presenterManager = PresenterManager.Instance;

        InitializeViewAndModel(view);
    }

    private void InitializeViewAndModel(IView view)
    {
        if (view is FieldObjectBase fieldObjectBase)
        {
            _view = fieldObjectBase;
        }
        else
        {
            throw new NullReferenceException("_view is not FieldObjectBase Type");
        }
        
        var modelType = _presenterManager.GetModelTypeUsingMatchDictionary(_view.GetType());
        var model = Activator.CreateInstance(modelType) as IModel;
        
        _model = model;
    }

    protected virtual void BindEvent()
    {
    }

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    // 

    #endregion
}