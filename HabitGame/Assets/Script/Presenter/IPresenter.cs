// note : PresenterManager가 사용할 수 있도록 오픈한다.
internal interface IPresenter
{
    public void Initialize(IView view);

    public void SetView();

    public void BindEvent();
}