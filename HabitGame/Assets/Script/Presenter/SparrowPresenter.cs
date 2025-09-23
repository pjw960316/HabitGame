using System;
using UniRx;
using UnityEngine;

public class SparrowPresenter : FieldObjectPresenterBase
{
    #region 1. Fields

    private const float COLLIDED_ROCK_ANIMATION_CHANGE_SECOND = 1f;
    
    
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

    // note : 충돌 개체 별 분기
    private void OnCollision(Collision collision)
    {
        var fieldObjectEnvironmentBase = collision.gameObject.GetComponent<FieldObjectBase>() as FieldObjectEnvironmentBase;

        switch (fieldObjectEnvironmentBase)
        {
            case FieldObjectRock:
                OnCollideWithRock();
                break;
            case FieldObjectMushroom:
            case FieldObjectFlower:
                OnCollideWithEatableEnvironment();
                break;
        }
    }

    private void OnCollideWithRock()
    {
        // log
        Debug.Log("Collide with Rock");
        
        // note : 박아서 스턴 된 애니메이션 의도
        _sparrowData.ChangeSparrowState(ESparrowState.FLY);

        Observable.Timer(TimeSpan.FromSeconds(COLLIDED_ROCK_ANIMATION_CHANGE_SECOND)).Subscribe(_ =>
        {
            _fieldObjectSparrow.RotateSparrow();
            _sparrowData.ChangeSparrowState(ESparrowState.WALK);
        }).AddTo(_disposable);
    }

    private void OnCollideWithEatableEnvironment()
    {
        // log
        Debug.Log("Collide with Flower 또는 Mushroom");
        
        _sparrowData.ChangeSparrowState(ESparrowState.EAT);
    }

    // note : model's ReactiveProperty Event
    public void OnChangeSparrowState(ESparrowState changedState)
    {
        //log
        Debug.Log($"reactiveProperty State Change Call : {changedState}");
        
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