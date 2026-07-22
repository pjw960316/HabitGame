using System;
using UniRx;
using UnityEngine;
using Observable = UniRx.Observable;

public class CameraController : ControllerBase
{
    #region 1. Fields

    private const float UNITY_DEV_ASPECT_RATIO = 1080f / 1920;
    private const float FOLLOWING_CAMERA_FOV = 86f;
    private const float FOLLOWING_CAMERA_UPDATE_MILLISECONDS = 10f;

    [SerializeField] private Camera _mainCamera;

    private Transform _mainCameraTransform;
    private IDisposable _followTargetObservable;

    private Vector3 _initializedMainCameraPosition;
    private Quaternion _initializedMainCameraRotation;
    private float _initializedMainCameraFOV;

    private readonly Vector3 FOLLOWING_FROM_BEHIND_CAMERA_ROTATE_ADJUST_VECTOR = new(0, -0.7f, -2);
    private readonly Vector3 FOLLOWING_FROM_BEHIND_CAMERA_POSITION_ADJUST_VECTOR = new(0, 1, -1);
    
    private readonly Vector3 FOLLOWING_FROM_ABOVE_CAMERA_ROTATE_ADJUST_VECTOR = Vector3.zero;
    private readonly Vector3 FOLLOWING_FROM_ABOVE_CAMERA_POSITION_ADJUST_VECTOR = new(0, 5, -3);

    public bool IsFollowingTarget;
    public Transform TargetTransform;
    #endregion

    #region 2. Properties
    //
    #endregion

    #region 3. Constructor

    protected override void Initialize()
    {
        RequestConnectManager<CameraManager, CameraController>(this);
        
        _mainCameraTransform = _mainCamera.transform;

        InitializeCameraFOV();
        CacheInitializedCameraData();
    }

    private void InitializeCameraFOV()
    {
        var originFOVDegree = _mainCamera.fieldOfView;
        var originTanFOV = Mathf.Tan(originFOVDegree * Mathf.Deg2Rad / 2f);

        var deviceAspect = Screen.width / (float)Screen.height;
        var aspectRatio = UNITY_DEV_ASPECT_RATIO / deviceAspect;

        var newFovDegree = 2f * Mathf.Atan(originTanFOV * aspectRatio) * Mathf.Rad2Deg;

        _mainCamera.fieldOfView = newFovDegree;
        _initializedMainCameraFOV = _mainCamera.fieldOfView;
    }

    private void CacheInitializedCameraData()
    {
        _initializedMainCameraPosition = _mainCameraTransform.position;
        _initializedMainCameraRotation = _mainCameraTransform.rotation;
    }

    #endregion

    #region 4. EventHandlers

    private void LateUpdate()
    {
        if (IsFollowingTarget)
        {
            if (TargetTransform == null)
            {
                Debug.LogError("Target Transform is not set.");
                return;
            }

            _mainCamera.fieldOfView = FOLLOWING_CAMERA_FOV;
            if (_mainCameraTransform == null || TargetTransform == null)
            {
                return;
            }

            var direction = TargetTransform.position - _mainCameraTransform.position - FOLLOWING_FROM_ABOVE_CAMERA_ROTATE_ADJUST_VECTOR;
            _mainCameraTransform.rotation = Quaternion.LookRotation(direction.normalized);
            _mainCameraTransform.position = TargetTransform.position + FOLLOWING_FROM_ABOVE_CAMERA_POSITION_ADJUST_VECTOR;
            
        }
    }

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    public void FollowTargetFromBehind(Transform targetTransform)
    {
        FollowTarget(targetTransform, FOLLOWING_FROM_BEHIND_CAMERA_ROTATE_ADJUST_VECTOR, FOLLOWING_FROM_BEHIND_CAMERA_POSITION_ADJUST_VECTOR);
    }

    public void FollowTargetFromAbove(Transform targetTransform)
    {
        FollowTarget(targetTransform, FOLLOWING_FROM_ABOVE_CAMERA_ROTATE_ADJUST_VECTOR, FOLLOWING_FROM_ABOVE_CAMERA_POSITION_ADJUST_VECTOR);
    }

    private void FollowTarget(Transform targetTransform, Vector3 rotateAdjustVector, Vector3 positionAdjustVector)
    {
        if (targetTransform == null)
        {
            Debug.LogError("Target Transform is not set.");
            return;
        }

        _mainCamera.fieldOfView = FOLLOWING_CAMERA_FOV;

        _followTargetObservable?.Dispose();
        _followTargetObservable = Observable
            .Interval(TimeSpan.FromMilliseconds(FOLLOWING_CAMERA_UPDATE_MILLISECONDS))
            .Subscribe(_ =>
            {
                if (_mainCameraTransform == null || targetTransform == null)
                {
                    return;
                }

                var direction = targetTransform.position - _mainCameraTransform.position - rotateAdjustVector;
                _mainCameraTransform.rotation = Quaternion.LookRotation(direction.normalized);
                _mainCameraTransform.position = targetTransform.position + positionAdjustVector;
            });
    }
    
    public void ReturnToDefaultCameraSetting()
    {
        _followTargetObservable?.Dispose();
        
        _mainCameraTransform.position = _initializedMainCameraPosition;
        _mainCameraTransform.rotation = _initializedMainCameraRotation;
        _mainCamera.fieldOfView = _initializedMainCameraFOV;
    }

    public Ray GetRay(Vector2 pos)
    {
        return _mainCamera.ScreenPointToRay(pos);
    }
    
    #endregion
}
