using UniRx;
using UnityEngine;

public class CameraManager : ManagerBase<CameraManager>, IHasController<CameraController>
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

    #region 3. Initialization

    public sealed override void Initialize()
    {
        _uiManager = UIManager.Instance;
        _fieldObjectManager = FieldObjectManager.Instance;
    }

    public sealed override void BindEvent()
    {
        _uiManager.OnOpenPopup
            .Subscribe(_ => { FollowSparrow(); })
            .AddTo(_followSparrowCameraMoveDisposable);

        _uiManager.OnClosePopup
            .Subscribe(_ =>
            {
                // WARNING
                // 바인드 시점에는 일단 _cameraController가 Null임.
                if(_cameraController != null)
                {
                    _cameraController.ReturnToDefaultCam();
                }
            }).AddTo(_followSparrowCameraMoveDisposable);
    }

    #endregion

    #region 4. Controller Registration

    // NOTE
    // Controller가 생길 때 이벤트로 콜이 들어온다.
    // 그러므로 Controller가 생성 될 때 초기화를 하니 좋은 설계라고 생각한다.
    public void RegisterController(CameraController cameraController)
    {
        if (cameraController == null)
        {
            Debug.LogError("CameraController is not set.");
            return;
        }
        
        _cameraController = cameraController;
    }

    #endregion

    #region 5. EventHandlers
    //
    #endregion

    #region 6. Methods

    private void FollowSparrow()
    {
        if (_cameraController == null)
        {
            Debug.LogError("CameraController is not registered.");
            return;
        }

        var randomSparrow = _fieldObjectManager.GetRandomSparrow();

        if (randomSparrow == null)
        {
            Debug.LogError("randomSparrow is not Selected.");
            return;
        }
        
        _cameraController.FollowTargetWithCloseUpCam(randomSparrow.FieldObjectTransform);
    }
    
    public void FollowPlayer(Transform playerTransform)
    {
        if (_cameraController == null)
        {
            Debug.LogError("CameraController is not registered.");
            return;
        }

        _cameraController.FollowPlayerWithSkyCam(playerTransform);
    }

    public Ray GetRay(Vector2 pos)
    {
        if (_cameraController == null)
        {
            Debug.LogError("CameraController is not registered.");
            return default;
        }

        return _cameraController.GetRay(pos);
    }

    #endregion
}
