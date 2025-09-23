using System;
using UniRx;
using UnityEngine;

public class SparrowPresenter : FieldObjectPresenterBase
{
    #region 1. Fields

    private FieldObjectSparrow _fieldObjectSparrow;
    private SparrowData _sparrowData;

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    public sealed override void Initialize(IView view)
    {
        base.Initialize(view);

        if (_view is FieldObjectSparrow sparrow)
        {
            _fieldObjectSparrow = sparrow;
        }

        ExceptionHelper.CheckNullException(_fieldObjectSparrow, "_fieldObjectSparrow is null");

        if (_model is SparrowData sparrowData)
        {
            _sparrowData = sparrowData;
        }

        ExceptionHelper.CheckNullException(_sparrowData, "_sparrowData is null");

        BindEvent();
    }


    protected sealed override void BindEvent()
    {
        base.BindEvent();

        _fieldObjectSparrow.OnCollision.Subscribe(OnCollision).AddTo(_disposable);
        _sparrowData.OnSparrowStateChanged.Subscribe(OnChangeSparrowState).AddTo(_disposable);
    }

    #endregion

    #region 4. EventHandlers

    private void OnCollision(Collision collision)
    {
        var fieldObjectBase = collision.gameObject.GetComponent<FieldObjectBase>();

        ExceptionHelper.CheckNullException(fieldObjectBase, "FieldObjectBase");

        if (fieldObjectBase is FieldObjectEnvironmentBase fieldObjectEnvironmentBase)
        {
            if (fieldObjectEnvironmentBase is FieldObjectRock)
            {
                OnCollideWithRock();
                
                Observable.Timer(TimeSpan.FromSeconds(1f)).Subscribe(_ =>
                {
                    _sparrowData.ChangeSparrowState(ESparrowState.WALK);
                });
            }
        }
    }

    private void OnCollideWithRock()
    {
        // note
        // 박아서 스턴 된 느낌
        _sparrowData.ChangeSparrowState(ESparrowState.FLY);
    }

    // note
    // model's ReactiveProperty Event
    public void OnChangeSparrowState(ESparrowState changedState)
    {
        //log
        Debug.Log($"reactiveProperty Event Call : {changedState}");
        
        var animIDKey = _sparrowData.SparrowStateAnimatorMatchDictionary[changedState];
        _fieldObjectSparrow.ChangeAnimation(animIDKey);
    }

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    // 

    #endregion
}