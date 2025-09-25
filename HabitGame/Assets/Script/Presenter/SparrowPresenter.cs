using System;
using UniRx;
using UnityEngine;
using Random = System.Random;

public class SparrowPresenter : FieldObjectPresenterBase
{
    #region 1. Fields

    private const float COLLIDED_ROCK_ANIMATION_CHANGE_SECOND = 1f;
    private const int FULL_ROTATION = 360;
    private const float ROTATION_CHANGE_INTERVAL_SECOND = 3f;

    private FieldObjectSparrow _fieldObjectSparrow;
    private SparrowData _sparrowData;

    private readonly CompositeDisposable _sparrowRandomPathDisposable = new();
    private readonly Random _randomMaker = new Random();

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
        var fieldObjectEnvironmentBase =
            collision.gameObject.GetComponent<FieldObjectBase>() as FieldObjectEnvironmentBase;

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

        _fieldObjectSparrow.StopSparrowMovePosition();
        _sparrowData.ChangeSparrowState(ESparrowState.FLY);

        // todo
        // 의도 되지 않은 타이밍에 콜 되는 거 막기
        Observable.Timer(TimeSpan.FromSeconds(COLLIDED_ROCK_ANIMATION_CHANGE_SECOND)).Subscribe(_ =>
        {
            _fieldObjectSparrow.RotateSparrowAndKeepDirection(FULL_ROTATION / 2);

            _sparrowData.ChangeSparrowState(ESparrowState.WALK);

            _fieldObjectSparrow.ChangeSparrowSpeed(1.2f);
        }).AddTo(_disposable);
    }

    private void OnCollideWithEatableEnvironment()
    {
        // log
        Debug.Log("Collide with Flower 또는 Mushroom");

        _sparrowData.ChangeSparrowState(ESparrowState.EAT);
    }
    
    public void OnChangeSparrowState(ESparrowState changedState)
    {
        _fieldObjectSparrow.ChangeAnimation((int)changedState);

        ChangeWalkDirectionRandomly(changedState);
    }

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    private void ChangeWalkDirectionRandomly(ESparrowState changedState)
    {
        _sparrowRandomPathDisposable?.Clear();
        
        if (changedState == ESparrowState.WALK)
        {
            Observable.Interval(TimeSpan.FromSeconds(ROTATION_CHANGE_INTERVAL_SECOND)).Subscribe(_ =>
            {
                var randDegree = _randomMaker.Next(0, FULL_ROTATION);
                _fieldObjectSparrow.RotateSparrowAndKeepDirection(randDegree);
            }).AddTo(_sparrowRandomPathDisposable);
        }
    }
    
    protected sealed override void DisposeCompositeDisposables()
    {
        base.DisposeCompositeDisposables();
        
        _sparrowRandomPathDisposable?.Dispose();
    }

    #endregion
}