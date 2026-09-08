using System;
using UniRx;
using UnityEngine;

// TODO
// 추후에는 Panel 자체를 Base로 만들고 abstract 화 시킬 지 모른다.
public class UIHorizontalPanel : MonoBehaviour, IView
{
    #region 1. Fields

    private PresenterManager _presenterManager;

    [SerializeField] private UIImageWidget _pointWidget;

    private readonly Subject<Unit> _onDestroyPanel = new();

    #endregion

    #region 2. Properties

    public IObservable<Unit> OnDestroyPanel => _onDestroyPanel;

    #endregion

    #region 3. Constructor

    private void Awake()
    {
        Initialize();

        CreatePresenterByManager();
    }

    private void Initialize()
    {
        _presenterManager = PresenterManager.Instance;
    }

    #endregion

    #region 4. EventHandlers

    private void OnDestroy()
    {
        _onDestroyPanel.OnNext(Unit.Default);
        _onDestroyPanel.OnCompleted();
        _onDestroyPanel.Dispose();
    }

    #endregion

    #region 5. Methods

    public void UpdatePoint(int point)
    {
        _pointWidget.SetText(point.ToString("N0"));
    }

    private void CreatePresenterByManager()
    {
        _presenterManager.CreatePresenter<HorizontalPanelPresenter>(this);
    }

    #endregion
}
