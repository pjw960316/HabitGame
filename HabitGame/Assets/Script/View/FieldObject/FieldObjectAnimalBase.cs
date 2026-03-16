using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public abstract class FieldObjectAnimalBase : FieldObjectBase
{
    #region 1. Fields

    private const string ANIMATOR_PARAMETER = "Animal";
    private const string URP_SHADER_COLOR = "_BaseColor";

    [SerializeField] private float _animalSpeed;
    [SerializeField] private Animator _animalAnimator;
    [SerializeField] private LODGroup _lodGroup;
    
    protected Rigidbody _animalRigidBody;
    protected Collision _currentCollision;
    protected Vector3 _animalWalkMovement;
    private Color _originColor;
    
    private int _animalIAnimatorIntegerParameter;
    private List<Renderer> _meshRendererList = new();
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

        _animalRigidBody = FieldObjectTransform.GetComponent<Rigidbody>();
        ExceptionHelper.CheckNullException(_animalRigidBody, "_rigidBody");

        _animalWalkMovement = FieldObjectTransform.forward * (_animalSpeed * Time.fixedDeltaTime);
        DefaultAnimalSpeed = _animalSpeed;

        InitializeRenderers();
    }

    private void InitializeRenderers()
    {
        var lodArr = _lodGroup.GetLODs();
        var lodLen = lodArr.Length;

        for (var idx = 0; idx < lodLen; idx++)
        {
            var targetRenderer = lodArr[idx].renderers.FirstOrDefault();

            if (targetRenderer != null)
            {
                _meshRendererList.Add(targetRenderer);
            }
        }

        if (_meshRendererList.Count == 0)
        {
            ExceptionHelper.CheckNullException(_meshRendererList[0] , "list zero ");
        }
        
        _originColor = _meshRendererList[0].material.color;
    }

    protected override void BindEvent()
    {
        base.BindEvent();
    }

    #endregion

    #region 4. EventHandlers

    private void FixedUpdate()
    {
        _animalRigidBody.MovePosition(FieldObjectTransform.position + _animalWalkMovement);
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
        FieldObjectTransform.Rotate(new Vector3(0, angle, 0));
        UpdateAnimalMovement();
    }

    public void RotateToFaceCollisionObject()
    {
        var path = _currentCollision.transform.position - FieldObjectTransform.position;
        var facePath = Quaternion.LookRotation(path);
        
        FieldObjectTransform.rotation = facePath;
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

    public void ChangeFieldObjectColor()
    {
        var meshRendererCount = _meshRendererList.Count;
        
        // todo : broadcast unirx로 수정.
        for (var idx = 0; idx < meshRendererCount; idx++)
        {
            var block = new MaterialPropertyBlock();
            block.SetColor(URP_SHADER_COLOR, Color.yellow);
            
            var meshRenderer = _meshRendererList[idx];
            meshRenderer.SetPropertyBlock(block);
        }
    }

    private void UpdateAnimalMovement()
    {
        _animalWalkMovement = FieldObjectTransform.forward * (_animalSpeed * Time.fixedDeltaTime);
    }

    #endregion
}