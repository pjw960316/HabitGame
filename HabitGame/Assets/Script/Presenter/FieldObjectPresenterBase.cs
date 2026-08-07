using System;
using UniRx;
using UnityEngine;

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

        RegisterAtFieldObjectManager();
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
        if (this is FieldObjectSparrowPresenter)
        {
            var gameObject = _fieldObjectBase.gameObject;
            var scene = gameObject.scene;

            Debug.Log(
                $"[SparrowPresenter][DESTROY] " +
                $"ViewName: {_fieldObjectBase.name}, " +
                $"ViewInstanceID: {_fieldObjectBase.InstanceID}, " +
                $"GameObjectInstanceID: {gameObject.GetInstanceID()}, " +
                $"PresenterHash: {GetHashCode()}, " +
                $"SceneName: {scene.name}, SceneHandle: {scene.handle}, " +
                $"Frame: {Time.frameCount}",
                _fieldObjectBase);
        }

        TerminateModel();

        TerminatePresenter();
    }

    #endregion

    #region 5. Methods

    // 


    private void TerminateModel()
    {
        _model?.Terminate();
    }
    
    private void RegisterAtFieldObjectManager()
    {
        if (this is FieldObjectSparrowPresenter)
        {
            var gameObject = _fieldObjectBase.gameObject;
            var scene = gameObject.scene;

            Debug.Log(
                $"[SparrowPresenter][REGISTER] " +
                $"ViewName: {_fieldObjectBase.name}, " +
                $"ViewInstanceID: {_fieldObjectBase.InstanceID}, " +
                $"GameObjectInstanceID: {gameObject.GetInstanceID()}, " +
                $"PresenterHash: {GetHashCode()}, " +
                $"SceneName: {scene.name}, SceneHandle: {scene.handle}, " +
                $"Frame: {Time.frameCount}\n" +
                $"CallStack:\n{Environment.StackTrace}",
                _fieldObjectBase);
        }

        _fieldObjectManager.RegisterFieldObjectPresenter(this);
    }

    public int GetFieldObjectInstanceID()
    {
        return _fieldObjectBase.InstanceID;
    }

    public Transform GetFieldObjectTransform()
    {
        return _fieldObjectBase.transform;
    }

    #endregion
}
