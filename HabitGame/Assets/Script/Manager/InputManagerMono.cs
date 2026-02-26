using UnityEngine;
using UnityEngine.InputSystem;

public class InputManagerMono : MonoBehaviour
{
    #region 1. Fields

    //

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    private void Awake()
    {
    }

    #endregion

    #region 4. EventHandlers

    private Vector2 moveInput;

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        Debug.Log(moveInput);
    }

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    // 

    #endregion
}