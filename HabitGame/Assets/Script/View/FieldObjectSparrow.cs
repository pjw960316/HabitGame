using System;
using UnityEngine;

public class FieldObjectSparrow : FieldObjectBase
{
    #region 1. Fields

    [SerializeField] private Transform _sparrowTransform;

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    // refactor 
    // base
    private void Awake()
    {
        
    }

    private void FixedUpdate()
    {
        _sparrowTransform.position += new Vector3(0.02f, 0, 0);
    }

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    // 

    #endregion
}