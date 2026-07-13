using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public abstract class FieldObjectAnimalBase : FieldObjectBase
{
    #region 1. Fields

    private const string URP_SHADER_COLOR = "_BaseColor";
    
    [SerializeField] private float _animalSpeed;
    [SerializeField] private LODGroup _lodGroup;
    
    protected Rigidbody _animalRigidBody;
    protected Collision _currentCollision;
    // NOTE
    // Animator를 사용하려면 해당 컴포넌트를 추가해야 한다.
private FieldObjectAnimator _fieldObjectAnimator;
   
    protected Vector3 _animalWalkMovement;
    private Color _originColor;
    
    private readonly List<Renderer> _meshRendererList = new();
    private readonly Subject<Collision> _onCollision = new();

    #endregion

    #region 2. Properties

    public IObservable<Collision> OnCollision => _onCollision;

    // NOTE
    // 절대 변경하지 마시오. readonly 문법이 불가능.
    public float DefaultAnimalSpeed { get; private set; }

    #endregion

    #region 3. Constructor

    protected override void Initialize()
    {
        base.Initialize();

        InitializeAnimator();

        _animalRigidBody = FieldObjectTransform.GetComponent<Rigidbody>();
        ExceptionHelper.CheckNullException(_animalRigidBody, "_rigidBody");

        _animalWalkMovement = FieldObjectTransform.forward * (_animalSpeed * Time.fixedDeltaTime);
        DefaultAnimalSpeed = _animalSpeed;

        InitializeRenderers();
    }

    private void InitializeAnimator()
    {
        if (TryGetComponent(out _fieldObjectAnimator))
        {
            _fieldObjectAnimator.Initialize();
        }
    }

    private void InitializeRenderers()
    {
        if (_lodGroup != null)
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
        }

        if (_meshRendererList.Count == 0)
        {
            _meshRendererList.AddRange(FieldObjectTransform.GetComponentsInChildren<Renderer>());
        }

        if (_meshRendererList.Count == 0)
        {
            ExceptionHelper.CheckNullException(null, "animal renderer list");
            return;
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
        if (_fieldObjectAnimator == null)
        {
            Debug.LogWarning($"{name} has no FieldObjectAnimator. Animation change skipped.");
            return;
        }

        _fieldObjectAnimator.ChangeAnimation(enumKey);
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

    public void ChangeFieldObjectColor(Color color)
    {
        var meshRendererCount = _meshRendererList.Count;
        
        // todo : broadcast unirx로 수정.
        for (var idx = 0; idx < meshRendererCount; idx++)
        {
            var block = new MaterialPropertyBlock();
            block.SetColor(URP_SHADER_COLOR, color);
            
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
