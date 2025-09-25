using System;
using UniRx;
using UnityEngine;

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
        
        if(_fieldObjectBase == null)
        {
            throw new InvalidCastException("_fieldObjectBase");
        }
    }
    
    private void InitializeModel()
    {
        var modelType = _presenterManager.GetModelTypeUsingMatchDictionary(_view.GetType());
        var model = Activator.CreateInstance(modelType) as IModel;
        
        _model = model;
    }

    protected virtual void BindEvent()
    {
        _fieldObjectBase.OnDestroyFieldObject.Subscribe(_ =>
        {
            OnOnDestroyFieldObject();
        });
    }

    #endregion

    #region 4. EventHandlers

    private void OnOnDestroyFieldObject()
    {
        TerminatePresenter();
    }

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    // 

    #endregion
}