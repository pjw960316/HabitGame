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
            // todo
            // 특별한 거랑 특별하지 않은 거 모두 구분
            if (fieldObjectEnvironmentBase is FieldObjectRock)
            {
                OnCollideWithRock();
                _sparrowData.ChangeSparrowState(ESparrowState.IDLE);

                Observable.Timer(TimeSpan.FromSeconds(1f)).Subscribe(_ =>
                {
                    _sparrowData.ChangeSparrowState(ESparrowState.WALK);

                });
            }

            //test
            //collisionDTO.OnChangedState.Invoke("ToIdle");
        }
    }

    private void OnCollideWithRock()
    {
        _fieldObjectSparrow.transform.Rotate(0f, 180f, 0f);
    }

    //test
    public void OnChangeSparrowState(ESparrowState changedState)
    {
        Debug.Log($"OnChangeSparrowState : {changedState}");
        if (changedState == ESparrowState.WALK)
        {
            _fieldObjectSparrow.ChangeAnimation("IsWalk");
        }
        if (changedState == ESparrowState.IDLE)
        {
            _fieldObjectSparrow.ChangeAnimation("IsIdle");
        }
    }

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    // 

    #endregion
}