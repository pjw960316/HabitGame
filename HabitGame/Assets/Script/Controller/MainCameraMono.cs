using System;
using UniRx;
using UnityEngine;
using Observable = UniRx.Observable;

public class MainCameraMono : MonoBehaviour
{
    #region 1. Fields

    private const float UNITY_DEV_ASPECT_RATIO = 1080f / 1920;
    private const float FOLLOWING_CAMERA_FOV = 86f;
    private const float FOLLOWING_CAMERA_UPDATE_MILLISECONDS = 10f;

    [SerializeField] private Camera _mainCamera;

    private CameraManager _cameraManager;
    private Transform _mainCameraTransform;
    private IDisposable _followFieldObjectObservable;

    private Vector3 _initializedMainCameraPosition;
    private Quaternion _initializedMainCameraRotation;
    private float _initializedMainCameraFOV;

    private readonly Vector3 FOLLOWING_CAMERA_ROTATE_ADJUST_VECTOR = new(0, -0.7f, -2);
    private readonly Vector3 FOLLOWING_CAMERA_POSITION_ADJUST_VECTOR = new(0, 1, -1);

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    private void Awake()
    {
        Initialize();

        _cameraManager.SetMainCamera(this);
    }

    private void Initialize()
    {
        _cameraManager = CameraManager.Instance;
        _mainCameraTransform = _mainCamera.transform;

        InitializeCameraFOV();

        CacheInitializedCameraData();
    }

    private void InitializeCameraFOV()
    {
        // note : 개발 단계에서 기획자가 최선의 각도를 맞춰 놓았을 것.
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

    //

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    public void UpdateToFollowFieldObject(Transform fieldObjectTransform)
    {
        _mainCamera.fieldOfView = FOLLOWING_CAMERA_FOV;

        _followFieldObjectObservable?.Dispose();
        _followFieldObjectObservable = Observable
            .Interval(TimeSpan.FromMilliseconds(FOLLOWING_CAMERA_UPDATE_MILLISECONDS))
            .Subscribe(_ =>
            {
                if (_mainCameraTransform == null)
                {
                    return;
                }

                var direction = fieldObjectTransform.position - _mainCameraTransform.position -
                                FOLLOWING_CAMERA_ROTATE_ADJUST_VECTOR;
                _mainCameraTransform.rotation = Quaternion.LookRotation(direction.normalized);
                _mainCameraTransform.position = fieldObjectTransform.position + FOLLOWING_CAMERA_POSITION_ADJUST_VECTOR;
            });
    }

    public void DisposeFollowSparrowCameraMoving()
    {
        _followFieldObjectObservable?.Dispose();
    }

    public void ReturnToDefaultCameraSetting()
    {
        _mainCameraTransform.position = _initializedMainCameraPosition;
        _mainCameraTransform.rotation = _initializedMainCameraRotation;
        _mainCamera.fieldOfView = _initializedMainCameraFOV;
    }

    #endregion
}