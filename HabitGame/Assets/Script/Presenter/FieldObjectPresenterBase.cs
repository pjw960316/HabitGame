using System;
using UniRx;

public abstract class FieldObjectPresenterBase : PresenterBase
{
    #region 1. Fields

    private FieldObjectBase _fieldObjectBase;

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    public override void Initialize(IView view)
    {
        base.Initialize(view);
    }

    protected override void InitializeView()
    {
        _fieldObjectBase = _view as FieldObjectBase;
        ExceptionHelper.CheckNullException(_fieldObjectBase,
            "_fieldObjectBase View is null in FieldObjectPresenterBase");
    }

    protected override void InitializeModel()
    {
        var modelType = _presenterManager.GetModelTypeUsingMatchDictionary(_view.GetType());
        var model = Activator.CreateInstance(modelType) as IModel;
        ExceptionHelper.CheckNullException(model, "model is null in FieldObjectPresenterBase");

        _model = model;
    }

    public override void BindEvent()
    {
        _fieldObjectBase.OnDestroyFieldObject.Subscribe(_ => { OnOnDestroyFieldObject(); });
    }

    #endregion

    #region 4. EventHandlers

    private void OnOnDestroyFieldObject()
    {
        TerminateModel();

        TerminatePresenter();
    }

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    private void TerminateModel()
    {
        _model?.Terminate();
    }

    #endregion
}