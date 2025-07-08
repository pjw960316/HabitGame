using UnityEngine;

public class AlarmPresenter : ButtonPresenterBase
{
    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    /* REFACTOR
     나는 지금 자식에서 constructor call 되면 부모의 default constructor 에서
     강제로 책임을 부여해서 AlarmPresenter로 캐스팅 하게 강제하고 싶은데 주석없이...*/
    public AlarmPresenter(IView view, IModel model)
    {
        //test
        Debug.Log("Create AlarmPresenter");
        
        View = view;
        Model = model;

        Initialize();
    }

    protected sealed override void Initialize()
    {
        base.Initialize();
        
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