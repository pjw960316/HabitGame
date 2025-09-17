using System;
using UniRx;
using UnityEngine;
using UnityEngine.Rendering;

public class FieldObjectSparrow : FieldObjectBase
{
    #region 1. Fields

    [SerializeField] protected Animator _sparrowAnimator;
    
    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    protected override void OnAwake()
    {
        base.OnAwake();
    }

    protected sealed override void Initialize()
    {
        base.Initialize();

        _sparrowAnimator.Play("Walk");

    }

    protected sealed override void InitializeEnumKey()
    {
        _eFieldObjectKey = EFieldObject.SPARROW;
    }

    protected sealed override void BindEvent()
    {
        //
    }

    #endregion

    #region 4. EventHandlers

    private void FixedUpdate()
    {
        _myFieldObjectTransform.position += new Vector3(-0.01f, 0, -0.01f);
    }

    private void OnCollisionEnter(Collision other)
    {
        _sparrowAnimator.Play("Idle_A");
        Observable.Timer(TimeSpan.FromSeconds(1)).Subscribe(_ =>
        {
            _sparrowAnimator.Play("Walk");
        });
    }

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    protected sealed override void CreatePresenterByManager()
    {
        _presenterManager.CreatePresenter2<SparrowPresenter>(this);
    }

    #endregion
}