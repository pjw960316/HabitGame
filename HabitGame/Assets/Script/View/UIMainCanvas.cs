using UnityEngine;

public class UIMainCanvas : MonoBehaviour, IView
{
    #region 1. Fields

    [SerializeField] private Canvas _mainCanvas;
    [SerializeField] private GameObject _toastMessage;

    #endregion

    #region 2. Properties

    public GameObject ToastMessage
    {
        get => _toastMessage;
        private set => _toastMessage = value;
    }

    #endregion

    #region 3. Constructor

    private void Awake()
    {
        OnAwake();
    }

    public void OnAwake()
    {
        UIToastManager.Instance.InjectMainCanvas(this);
    }

    #endregion

    #region 4. Methods

    // default

    #endregion

    #region 5. EventHandlers

    // default

    #endregion

    public void BindEvent()
    {
        //
    }
}