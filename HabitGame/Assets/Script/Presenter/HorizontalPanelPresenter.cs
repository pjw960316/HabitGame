using UniRx;

public class HorizontalPanelPresenter : PresenterBase
{
    #region 1. Fields

    private UIHorizontalPanel _uiHorizontalPanel;
    private HorizontalPanelData _horizontalPanelData;

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    public sealed override void Initialize(IView view)
    {
        base.Initialize(view);
    }
    
    protected sealed override void InitializeView()
    {
        _uiHorizontalPanel = _view as UIHorizontalPanel;
        ExceptionHelper.CheckNullException(_uiHorizontalPanel, "_uiHorizontalPanel");
    }

    protected sealed override void InitializeModel()
    {
        _horizontalPanelData = new HorizontalPanelData();
        _model = _horizontalPanelData;
    }

    public sealed override void SetView()
    {
        _uiHorizontalPanel.UpdatePoint(_myCharacterManager.GetMonthlySiestaMoney());
    }

    public sealed override void BindEvent()
    {
        _myCharacterManager.OnUpdateSiestaRecord
            .Subscribe(OnUpdateSiestaRecord)
            .AddTo(_disposable);
        
        // TODO 
        // Terminate는 내 시스템에서 다시 구성
        //_uiHorizontalPanel.OnDestroyPanel.Subscribe(_ => OnDestroyPanel()).AddTo(_disposable);
    }

    #endregion

    #region 4. EventHandlers

    private void OnUpdateSiestaRecord(int todaySiestaMoney)
    {
        _uiHorizontalPanel.UpdatePoint(todaySiestaMoney);
    }

   /* private void OnDestroyPanel()
    {
        _model?.Terminate();
        TerminatePresenter();
    }*/

    #endregion

    #region 5. Methods

    //

    #endregion
}
