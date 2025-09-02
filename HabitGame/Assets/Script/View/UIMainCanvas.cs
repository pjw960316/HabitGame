using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UIMainCanvas : MonoBehaviour, IView
{
    #region 1. Fields

    [SerializeField] private Canvas _mainCanvas;
    [SerializeField] private Image _toastMessage;
    [SerializeField] private TextMeshProUGUI _toastText;

    #endregion

    #region 2. Properties

    public Canvas MainCanvas => _mainCanvas;
    public Image ToastMessage => _toastMessage;
    public TextMeshProUGUI ToastText => _toastText;

    #endregion

    #region 3. Constructor

    private void Awake()
    {
        OnAwake();
    }

    public void OnAwake()
    {
        Initialize();
    }

    private void Initialize()
    {
        UIManager.Instance.InjectMainCanvas(this);
        UIToastManager.Instance.InjectMainCanvas(this);
        
        _toastMessage.gameObject.SetActive(false);
    }

    #endregion

    #region 4. Methods

    // default

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}