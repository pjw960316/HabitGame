using System;
using UniRx;
using UnityEngine;

public class FieldObjectSparrow : FieldObjectBase
{
    #region 1. Fields

    [SerializeField] protected Animator _sparrowAnimator;
    private readonly Subject<Collision> _onCollision = new();

    #endregion

    #region 2. Properties

    public IObservable<Collision> OnCollision => _onCollision;

    #endregion

    #region 3. Constructor

    protected sealed override void Initialize()
    {
        base.Initialize();
    }

    protected override void InitializeEnumFieldObjectKey()
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
        _onCollision.OnNext(other);
    }

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    protected sealed override void CreatePresenterByManager()
    {
        _presenterManager.CreateFieldObjectPresenter<SparrowPresenter>(this);
    }

    #endregion

    #region 7. Animation Methods

    // todo
    // dictionary로 관리해서 key로 받고 하는.
    public void ChangeAnimation()
    {
        //_sparrowAnimator.SetTrigger();
    }

    #endregion
}