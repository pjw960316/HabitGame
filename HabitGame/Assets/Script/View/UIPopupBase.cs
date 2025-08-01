using UnityEngine;

public abstract class UIPopupBase : MonoBehaviour, IView
{
    protected UIManager _uiManager;
    protected SoundManager _soundManager;
    
    private void Awake()
    {
        OnAwake();
    }

    public virtual void OnAwake()
    {
        _soundManager = SoundManager.Instance;
        _uiManager = UIManager.Instance;
        
        BindEvent();
    }

    private void BindEvent()
    {
    }


    public void CreatePresenterByManager()
    {
    }
}