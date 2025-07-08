/* TODO
 View가 많은 데 모두 1:1 대응해서 Presenter를 만들면 답이 없다.
 그러므로 Presenter를 추상적으로 상속 단계를 만드는 것
 PresenterBase는 모든 Presenter라면 있어야 하는 기능
 그 아래는 ButtonPresenterBase -> 버튼 View랑 연관된 Presenter
 그 아래는 AlarmPresenterBase -> 점점 특수한 Button과 연관 되는 것.*/

using UniRx;

public abstract class PresenterBase : IPresenter
{
    #region 1. Fields

    protected IView View;
    protected IModel Model;
    protected readonly CompositeDisposable Disposable = new();

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    // refactor
    // 생성 될 때 IView와 IModel을 받도록 책임은 부여했다.
    // 그러나 이걸 콜 하는 건 별개다. 왜냐 부모니까. Default가 아니거든.
    protected PresenterBase()
    {
    }

    // default

    #endregion

    #region 4. Methods

    protected virtual void Initialize()
    {
        
    }
    //protected abstract void CastAbstractModel();
    //protected abstract void CastAbstractView();

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}