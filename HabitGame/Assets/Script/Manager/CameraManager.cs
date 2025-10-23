using UniRx;
using UnityEngine;

public class CameraManager : ManagerBase<CameraManager>
{
    #region 1. Fields

    private MainCameraMono _mainCamera;

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

    public void SetMainCamera(MainCameraMono mainCameraMono)
    {
        _mainCamera = mainCameraMono;
    }

    private void BindEvent()
    {
        _uiManager.OnOpenPopup
            .Subscribe(_ =>
            {
                var randomSparrow = _fieldObjectManager.GetRandomSparrow();
                SetMainCameraToFollowSparrow(randomSparrow);
            })
            .AddTo(_followSparrowCameraMoveDisposable);

        _uiManager.OnClosePopup.Subscribe(_ =>
        {
            _mainCamera.DisposeFollowSparrowCameraMoving();

            ReturnToDefaultCameraSetting();
        });
    }

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    //

    #endregion

    #region 6. Methods

    private void SetMainCameraToFollowSparrow(FieldObjectBase fieldObjectBase)
    {
        if (fieldObjectBase == null)
        {
            Debug.Log("FieldObject is Destroyed -> Not change Camera FOV");
            return;
        }

        var fieldObjectTransform = fieldObjectBase.transform;

        _mainCamera.UpdateToFollowFieldObject(fieldObjectTransform);
    }

    private void ReturnToDefaultCameraSetting()
    {
        _mainCamera.ReturnToDefaultCameraSetting();
    }

    #endregion
}