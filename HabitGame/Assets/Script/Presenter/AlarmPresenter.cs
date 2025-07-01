public class AlarmPresenter : IPresenter
{
    private IView _view;
    private readonly IModel _model;

    public AlarmPresenter(IView view, IModel model)
    {
        _view = view;
        _model = model;
    }

    // TEST : 제거 해라.
    public int TestModelViewConnection()
    {
        if (_model is SoundData soundData)
        {
            return soundData.testValue;
        }

        return 1;
    }
}