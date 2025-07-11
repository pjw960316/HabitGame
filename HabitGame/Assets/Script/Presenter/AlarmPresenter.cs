using UnityEngine;

public class AlarmPresenter : PresenterBase
{
    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor
    
    public AlarmPresenter()
    {
    }

    public sealed override void Initialize(IView view)
    {
        base.Initialize(view);

        BindEvent();
    }

    #endregion

    #region 4. Methods

    private void BindEvent()
    {
        //_view.OnSoundButtonClicked.Subscribe(unit => OpenPopup()).AddTo(_disposable);
    }

    #endregion
}