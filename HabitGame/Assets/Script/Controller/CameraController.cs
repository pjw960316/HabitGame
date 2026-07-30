using UnityEngine;

public class CameraController : ControllerBase
{
    #region 1. Fields

    private const float UNITY_DEV_ASPECT_RATIO = 1080f / 1920;

    [SerializeField] private Camera _mainCamera;

    // TODO
    // 여러 모드가 존재하면 Enum을 고려한다.

    [Header("Default Sky Cam")]
    [SerializeField] private bool _isDefaultSkyCamTestMode;
    [SerializeField, Range(1f, 179f)] private float _defaultSkyCamFov;
    [SerializeField] private Vector3 _defaultSkyCamPositionOffset;
    [SerializeField] private Vector3 _defaultSkyCamLookOffset;

    [Header("Close-Up Follow Cam")]
    [SerializeField, Range(1f, 179f)] private float _closeUpFollowCamFov;
    [SerializeField] private Vector3 _closeUpFollowCamPositionOffset;
    [SerializeField] private Vector3 _closeUpFollowCamLookOffset;

    [Header("Player Follow Sky Cam")]
    [SerializeField, Range(1f, 179f)] private float _playerFollowSkyCamFov;
    [SerializeField] private Vector3 _playerFollowSkyCamPositionOffset;
    [SerializeField] private Vector3 _playerFollowSkyCamLookOffset;

    private Transform _mainCameraTransform;
    private Transform _targetTransform;

    private Vector3 _currentPositionOffset;
    private Vector3 _currentLookOffset;

    private Vector3 _initializedMainCameraPosition;
    private Quaternion _initializedMainCameraRotation;
    private float _initializedMainCameraFov;

    #endregion

    #region 2. Properties
    //
    #endregion

    #region 3. Constructor

    protected override void Initialize()
    {
        RequestConnectManager<CameraManager, CameraController>(this);
        
        _mainCameraTransform = _mainCamera.transform;

        InitializeDefaultCameraView();
    }

    private void InitializeDefaultCameraView()
    {
        _mainCamera.fieldOfView = GetAdjustedCameraFOV(_defaultSkyCamFov);
        _mainCameraTransform.position = _defaultSkyCamPositionOffset;

        var defaultLookDirection = _defaultSkyCamLookOffset - _defaultSkyCamPositionOffset;
        if (defaultLookDirection != Vector3.zero)
        {
            _mainCameraTransform.rotation = Quaternion.LookRotation(defaultLookDirection);
        }
        
        // NOTE
        // 초기 상태 캐싱
        _initializedMainCameraPosition = _mainCameraTransform.position;
        _initializedMainCameraRotation = _mainCameraTransform.rotation;
        _initializedMainCameraFov = _mainCamera.fieldOfView;
    }

    private float GetAdjustedCameraFOV(float originFOVDegree)
    {
        var originTanFOV = Mathf.Tan(originFOVDegree * Mathf.Deg2Rad / 2f);

        var deviceAspect = Screen.width / (float)Screen.height;
        var aspectRatio = UNITY_DEV_ASPECT_RATIO / deviceAspect;

        return 2f * Mathf.Atan(originTanFOV * aspectRatio) * Mathf.Rad2Deg;
    }

    #endregion

    #region 4. EventHandlers

    private void LateUpdate()
    {
        // TEST
        // Inspector에서 각도 조절할 때 키면 된다.
        if (_isDefaultSkyCamTestMode)
        {
            _mainCamera.fieldOfView = GetAdjustedCameraFOV(_defaultSkyCamFov);
            _mainCameraTransform.position = _defaultSkyCamPositionOffset;

            var defaultLookDirection = _defaultSkyCamLookOffset - _mainCameraTransform.position;
            if (defaultLookDirection != Vector3.zero)
            {
                _mainCameraTransform.rotation = Quaternion.LookRotation(defaultLookDirection);
            }

            return;
        }

        if (_targetTransform == null)
        {
            return;
        }

        var targetPosition = _targetTransform.position;

        _mainCameraTransform.position = targetPosition + _currentPositionOffset;

        var direction = targetPosition + _currentLookOffset - _mainCameraTransform.position;
        if (direction == Vector3.zero)
        {
            return;
        }

        _mainCameraTransform.rotation = Quaternion.LookRotation(direction);
    }

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    public void FollowTargetWithCloseUpCam(Transform targetTransform)
    {
        Debug.Log($"CloseUp Cam + {targetTransform.name}");
        
        FollowTarget(
            targetTransform,
            _closeUpFollowCamFov,
            _closeUpFollowCamPositionOffset,
            _closeUpFollowCamLookOffset);
    }

    public void FollowPlayerWithSkyCam(Transform playerTransform)
    {
        Debug.Log($"SkyCam + {playerTransform.name}");
        
        FollowTarget(
            playerTransform,
            _playerFollowSkyCamFov,
            _playerFollowSkyCamPositionOffset,
            _playerFollowSkyCamLookOffset);
    }

    private void FollowTarget(
        Transform targetTransform,
        float cameraFOV,
        Vector3 positionOffset,
        Vector3 lookOffset)
    {
        if (targetTransform == null)
        {
            Debug.LogError("Target Transform is not set.");
            return;
        }

        _targetTransform = targetTransform;
        _currentPositionOffset = positionOffset;
        _currentLookOffset = lookOffset;
        _mainCamera.fieldOfView = GetAdjustedCameraFOV(cameraFOV);
    }
    
    public void ReturnToDefaultCam()
    {
        _targetTransform = null;
        
        _mainCameraTransform.position = _initializedMainCameraPosition;
        _mainCameraTransform.rotation = _initializedMainCameraRotation;
        _mainCamera.fieldOfView = _initializedMainCameraFov;
    }

    public Ray GetRay(Vector2 pos)
    {
        return _mainCamera.ScreenPointToRay(pos);
    }
    
    #endregion
}
