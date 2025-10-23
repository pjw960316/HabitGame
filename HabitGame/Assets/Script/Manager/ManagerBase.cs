using System.Collections.Generic;

public abstract class ManagerBase<T> : IManager
    where T : class, new()
{
    #region 1. Fields

    private static T _instance;

    #endregion

    #region 2. Properties

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new T();
            }

            return _instance;
        }
    }

    #endregion

    #region 3. Constructor

    //

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    public virtual void PreInitialize()
    {
    }

    public virtual void Initialize()
    {
    }

    public virtual void LateInitialize()
    {
    }

    public virtual void SetModel(IEnumerable<IModel> models)
    {
    }

    public virtual void BindEvent()
    {
    }

    public void ConnectInstanceByActivator(IManager instance)
    {
        _instance = instance as T;
    }

    #endregion
}