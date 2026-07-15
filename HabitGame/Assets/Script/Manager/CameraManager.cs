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
                // WARNING
                // 바인드 시점에는 일단 _cameraController가 Null임.
                if(_cameraController != null)
                {
                    _cameraController.ReturnToDefaultCameraSetting();
                }
            }).AddTo(_followSparrowCameraMoveDisposable);
    }

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

    #region 4. EventHandlers
    //
    #endregion

    #region 5. Request Methods

    private void RequestFollowSparrow()
    {
        if (_cameraController == null)
        {
            Debug.LogError("CameraController is not registered.");
            return;
        }

        var randomSparrow = _fieldObjectManager.GetRandomSparrow();

        _cameraController.StartFollowFieldObject(randomSparrow.transform);
    }

    #endregion

    #region 6. Methods

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
