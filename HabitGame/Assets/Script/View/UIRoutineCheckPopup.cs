using System.Collections.Generic;
using UnityEngine;

public class UIRoutineCheckPopup : UIPopupBase
{
    #region 1. Fields

    [SerializeField] private List<UIToggleBase> _toggleList = new();

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor
    public override void OnAwake()
    {
        base. OnAwake();
        
        BindEvent();
        
    }

    #endregion

    #region 4. Methods

    private void BindEvent()
    {
        
    }

    #endregion

    #region 5. EventHandlers

    // default

    #endregion
    
    
    
    
}
