using System;
using UniRx;
using UnityEngine;
using Observable = UniRx.Observable;

public class CameraController : MonoBehaviour
{
    #region 1. Fields

    private const float UNITY_DEV_ASPECT_RATIO = 1080f / 1920;
    private const float FOLLOWING_CAMERA_FOV = 86f;
    private const float FOLLOWING_CAMERA_UPDATE_MILLISECONDS = 10f;

    [SerializeField] private Camera _mainCamera;

    // NOTE
    // 자신의 매니저를 들고 있는다.
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
    }

    private void Initialize()
    {
        // todo : 
        // controller가 manager를 들고 있지 않는 구조가 좋긴한데.
        // 일단 하고, 필요시에 rx로 
        _cameraManager = CameraManager.Instance;
        
        if (_cameraManager == null)
        {
            Debug.LogError("카메라 매니저가 null 입니다.");

            return;
        }
        
        _cameraManager.SetCameraController(this);
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

    //

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    public void StartFollowFieldObject(Transform fieldObjectTransform)
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
    
    public void ReturnToDefaultCameraSetting()
    {
        _followFieldObjectObservable?.Dispose();
        
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
