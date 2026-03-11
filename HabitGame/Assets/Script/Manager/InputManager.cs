
// note : 라우팅

using System;
using UnityEngine;

public class InputManager : ManagerBase<InputManager>
{
    #region 1. Fields

    private Vector2 _moveVector;

    #endregion

    #region 2. Properties

    public Vector2 MoveVector => _moveVector;

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

    public void UpdateMoveVector(Vector2 vector)
    {
        _moveVector = vector;
    }

    #endregion
}