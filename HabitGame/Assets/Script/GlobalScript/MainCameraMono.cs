using UnityEngine;

public class MainCameraMono : MonoBehaviour
{
    #region 1. Fields

    [SerializeField] private Camera _mainCamera;
    
    private CameraManager _cameraManager;
    private Transform _mainCameraTransform;
    
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

    public void FollowFieldObject(Transform fieldObjectTransform)
    {
        _mainCameraTransform.LookAt(fieldObjectTransform);
    }

    #endregion
}