using System;
using UniRx;
using UnityEngine;

public abstract class FieldObjectAnimalBase : FieldObjectBase
{
    #region 1. Fields

    [SerializeField] private float _animalSpeed;

    protected Rigidbody _animalRigidBody;
    protected Collision _currentCollision;
    protected Vector3 _animalWalkMovement;

    private readonly Subject<Collision> _onCollision = new();

    #endregion

    #region 2. Properties

    public IObservable<Collision> OnCollision => _onCollision;

    // note : 절대 변경하지 마시오. readonly 문법이 불가능.
    public float DefaultAnimalSpeed { get; private set; }

    #endregion

    #region 3. Constructor

    protected override void Initialize()
    {
        base.Initialize();

        _animalRigidBody = _myFieldObjectTransform.GetComponent<Rigidbody>();
        ExceptionHelper.CheckNullException(_animalRigidBody, "_rigidBody");

        _animalWalkMovement = _myFieldObjectTransform.forward * (_animalSpeed * Time.fixedDeltaTime);
        DefaultAnimalSpeed = _animalSpeed;
    }

    protected override void BindEvent()
    {
        base.BindEvent();
    }

    #endregion

    #region 4. EventHandlers

    private void FixedUpdate()
    {
        // refactor
        _animalRigidBody.MovePosition(_myFieldObjectTransform.position + _animalWalkMovement);
    }

    private void OnCollisionEnter(Collision other)
    {
        _currentCollision = other;

        _onCollision.OnNext(other);
    }

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    protected sealed override void CreatePresenterByManager()
    {
        // refactor
        // 이걸 presenterBase?
        _presenterManager.CreatePresenter<SparrowPresenter>(this);
    }

    //refactor
    //이것만
    /*public void ChangeAnimation(int enumKey)
    {
        _sparrowAnimator.SetInteger(_sparrowAnimatorParameter, enumKey);
    }*/

    public void ChangeAnimalPath(int angle)
    {
        _myFieldObjectTransform.Rotate(new Vector3(0, angle, 0));
        UpdateAnimalMovement();
    }

    public void RotateToFaceCollisionObject()
    {
        var path = _currentCollision.transform.position - _myFieldObjectTransform.position;
        var facePath = Quaternion.LookRotation(path);
        _myFieldObjectTransform.rotation = facePath;
    }

    public void ChangeAnimalSpeed(float speed)
    {
        _animalSpeed = speed;

        UpdateAnimalMovement();
    }

    public void ChangeAnimalDefaultSpeed()
    {
        ChangeAnimalSpeed(DefaultAnimalSpeed);
    }

    public void ChangeAnimalSpeedZero()
    {
        ChangeAnimalSpeed(0f);
    }

    private void UpdateAnimalMovement()
    {
        _animalWalkMovement = _myFieldObjectTransform.forward * (_animalSpeed * Time.fixedDeltaTime);
    }

    public void StopAnimalMoving()
    {
        ChangeAnimalSpeed(0f);
    }

    #endregion
}