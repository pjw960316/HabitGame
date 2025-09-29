using System.Collections.Generic;

public class CameraManager : ManagerBase<CameraManager>, IManager
{
    #region 1. Fields

    private MainCameraMono _mainCamera;

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    public void PreInitialize()
    {
        //
    }

    public void Initialize()
    {
        //
    }

    public void SetModel(IEnumerable<IModel> models)
    {
        //
    }

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    public void RequestMainCameraToFollowFieldObject(FieldObjectBase fieldObjectBase)
    {
        var fieldObjectTransform = fieldObjectBase.transform;

        _mainCamera.FollowFieldObject(fieldObjectTransform);
    }

    #endregion

    #region 6. Methods

    // refactor
    // _mainCamera는 null 일 수 있지 않은가.
    public void SetMainCamera(MainCameraMono mainCameraMono)
    {
        _mainCamera = mainCameraMono;
    }

    #endregion
}