using UniRx;
using UnityEngine;

public class CameraManager : ManagerBase<CameraManager>
{
    #region 1. Fields

    private CameraController _cameraController;

    private UIManager _uiManager;
    private FieldObjectManager _fieldObjectManager;

    private readonly CompositeDisposable _followSparrowCameraMoveDisposable = new();

    #endregion

    #region 2. Properties
    //
    #endregion

    #region 3. Constructor

    public sealed override void Initialize()
    {
        _uiManager = UIManager.Instance;
        _fieldObjectManager = FieldObjectManager.Instance;
    }

    public sealed override void BindEvent()
    {
        _uiManager.OnOpenPopup
            .Subscribe(_ => { RequestFollowSparrow(); })
            .AddTo(_followSparrowCameraMoveDisposable);

        _uiManager.OnClosePopup
            .Subscribe(_ =>
            {
                _cameraController.ReturnToDefaultCameraSetting();
            }).AddTo(_followSparrowCameraMoveDisposable);
    }

    public void SetCameraController(CameraController cameraController)
    {
        if (cameraController == null)
        {
            Debug.LogError("CameraController is not set.");
            return;
        }
        
        _cameraController = cameraController;
    }

    #endregion

    #region 4. EventHandlers
    //
    #endregion

    #region 5. Request Methods

    private void RequestFollowSparrow()
    {
        var randomSparrow = _fieldObjectManager.GetRandomSparrow();

        _cameraController.StartFollowFieldObject(randomSparrow.transform);
    }

    #endregion

    #region 6. Methods

    public Ray GetRay(Vector2 pos)
    {
        return _cameraController.GetRay(pos);
    }

    #endregion
}