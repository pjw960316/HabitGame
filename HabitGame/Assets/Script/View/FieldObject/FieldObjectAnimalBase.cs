using System;
using UniRx;
using UnityEngine;

public abstract class FieldObjectAnimalBase : FieldObjectBase
{
    #region 1. Fields

    private const string ANIMATOR_PARAMETER = "Animal";

    [SerializeField] private float _animalSpeed;
    [SerializeField] private Animator _animalAnimator;

    protected Rigidbody _animalRigidBody;
    protected Collision _currentCollision;
    protected Vector3 _animalWalkMovement;

    private int _animalIAnimatorIntegerParameter;
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

        _animalIAnimatorIntegerParameter = Animator.StringToHash(ANIMATOR_PARAMETER);

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

    protected override void CreatePresenterByManager()
    {
    }

    public void ChangeAnimation(int enumKey)
    {
        _animalAnimator.SetInteger(_animalIAnimatorIntegerParameter, enumKey);
    }

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

    #endregion
}