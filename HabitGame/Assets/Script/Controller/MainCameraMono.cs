using System;
using UniRx;
using UnityEngine;
using Observable = UniRx.Observable;

public class MainCameraMono : MonoBehaviour
{
    #region 1. Fields

    [SerializeField] private Camera _mainCamera;
    
    private CameraManager _cameraManager;
    private Transform _mainCameraTransform;
    private IDisposable _followFieldObjectObservable;

    private Vector3 _initializedMainCameraPosition;
    private Quaternion _initializedMainCameraRotation;
    private float _initializedMainCameraFOV;
    
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

        InitializeCamera();
        CacheInitializedCameraData();
    }

    private void InitializeCamera()
    {
        
    }
    private void CacheInitializedCameraData()
    {
        _initializedMainCameraPosition = _mainCameraTransform.position;
        _initializedMainCameraRotation = _mainCameraTransform.rotation;
        _initializedMainCameraFOV = _mainCamera.fieldOfView;
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
        //_mainCameraTransform.LookAt(fieldObjectTransform);
        _mainCamera.fieldOfView = 86f;
        
        _followFieldObjectObservable?.Dispose();
        _followFieldObjectObservable = Observable.Interval(TimeSpan.FromMilliseconds(10f)).Subscribe(_ =>
        {
            Vector3 direction = fieldObjectTransform.position - new Vector3(0,-0.7f,-2) - _mainCameraTransform.position;
            _mainCameraTransform.rotation = Quaternion.LookRotation(direction.normalized);
            _mainCameraTransform.position = fieldObjectTransform.position + new Vector3(0, 1, -1);
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