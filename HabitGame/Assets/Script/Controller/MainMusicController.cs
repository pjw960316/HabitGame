using System;

public class MainMusicController : SoundControllerBase
{
    #region 1. Fields

    //

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor
    
    protected override void Initialize()
    {
        base.Initialize();

        _soundManager.SetMainMusicPlayerController(this);
        _soundManager.PlayBackgroundMusic();
    }

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    // 

    #endregion
}