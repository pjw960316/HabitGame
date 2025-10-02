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
    }

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    // note : UI가 떠 있을 때.
    public void FollowFieldObject(Transform fieldObjectTransform)
    {
        //_mainCameraTransform.LookAt(fieldObjectTransform);
        _mainCamera.fieldOfView = 60f;
        
        _followFieldObjectObservable = Observable.Interval(TimeSpan.FromMilliseconds(10f)).Subscribe(_ =>
        {
            Vector3 direction = fieldObjectTransform.position - new Vector3(1,0,-2) - _mainCameraTransform.position;
            _mainCameraTransform.rotation = Quaternion.LookRotation(direction.normalized);
            _mainCameraTransform.position = fieldObjectTransform.position + new Vector3(0, 1, -1);
        });
        Debug.Log($"{fieldObjectTransform.gameObject.name}");
    }

    public void DisposeFollowFieldObjectInterval()
    {
        _followFieldObjectObservable?.Dispose();
    }

    #endregion
}