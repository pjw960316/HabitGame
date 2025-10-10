using System;
using UniRx;

// note
// UI Presenter & Field Presenter의 상위 타입
// View를 받아서 생성.
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

        CastView();

        InitializeModel();
    }

    private void CastView()
    {
        _fieldObjectBase = _view as FieldObjectBase;
        ExceptionHelper.CheckNullException(_fieldObjectBase,
            "_fieldObjectBase View is null in FieldObjectPresenterBase");
    }

    private void InitializeModel()
    {
        var modelType = _presenterManager.GetModelTypeUsingMatchDictionary(_view.GetType());
        var model = Activator.CreateInstance(modelType) as IModel;
        ExceptionHelper.CheckNullException(model, "model is null in FieldObjectPresenterBase");

        _model = model;
    }

    protected virtual void BindEvent()
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