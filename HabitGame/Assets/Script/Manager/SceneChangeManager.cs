using UnityEngine.SceneManagement;

public sealed class SceneChangeManager : ManagerBase<SceneChangeManager>
{
    #region 1. Fields

    //NOTE : 아직 VAli 필요 없음
    //private readonly SceneNameValidator _sceneNameValidator = new();
    private readonly SceneLoader _sceneLoader = new();

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    //

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    public void LoadScene(string sceneName)
    {
        //_sceneNameValidator.Validate(sceneName);
        _sceneLoader.Load(sceneName);
    }

    #endregion

    #region 6. Methods

    //

    #endregion

    // private sealed class SceneNameValidator
    // {
    //     public void Validate(string sceneName)
    //     {
    //         if (string.IsNullOrWhiteSpace(sceneName))
    //         {
    //             throw new ArgumentException("sceneName is null or empty", nameof(sceneName));
    //         }
    //     }
    // }

    private sealed class SceneLoader
    {
        public void Load(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}