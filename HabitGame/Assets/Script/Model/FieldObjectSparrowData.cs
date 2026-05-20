public class FieldObjectSparrowData : FieldObjectAnimalData
{
    #region 1. Fields
    
    private bool _isTarget = false;
    
    #endregion

    #region 2. Properties
    
    public bool IsTarget => _isTarget;
    
    #endregion

    #region 3. Constructor

    public FieldObjectSparrowData()
    {
    }
    
    #endregion

    #region 4. EventHandlers

    private void OnTargetSelected()
    {
        ChangeAnimalState(EAnimalState.FLY);
    }

    private void OnTargetDeselected()
    {
        ChangeAnimalState(EAnimalState.WALK);
    }
    #endregion

    #region 5. Request Methods
    // 
    #endregion

    #region 6. Methods

    public void UpdateTarget(bool isTarget)
    {
        _isTarget = isTarget;

        if (isTarget)
        {
            OnTargetSelected();
        }
        else
        {
            OnTargetDeselected();
        }
    }
    
    #endregion
}