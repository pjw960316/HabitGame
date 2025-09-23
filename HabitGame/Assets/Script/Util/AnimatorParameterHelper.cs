using System.Collections.Generic;
using UnityEngine;

public enum EAnimatorParams
{
    WALK,
    IDLE,
    RUN,
    EAT,
    FLY,
    JUMP,
    SIT,
    SPIN
}

public static class AnimatorParameterHelper
{
    #region 1. Fields

    private static readonly Dictionary<EAnimatorParams, int> _animatorParameterDictionary = new();

    #endregion

    #region 2. Properties

    public static Dictionary<EAnimatorParams, int> AnimatorParameterDictionary => _animatorParameterDictionary;

    #endregion

    #region 3. Constructor

    static AnimatorParameterHelper()
    {
        Initialize();
    }

    private static void Initialize()
    {
        _animatorParameterDictionary[EAnimatorParams.WALK] = Animator.StringToHash("IsWalk");
        _animatorParameterDictionary[EAnimatorParams.IDLE] = Animator.StringToHash("IsIdle");
        _animatorParameterDictionary[EAnimatorParams.RUN] = Animator.StringToHash("IsRun");
        _animatorParameterDictionary[EAnimatorParams.EAT] = Animator.StringToHash("IsEat");
        _animatorParameterDictionary[EAnimatorParams.FLY] = Animator.StringToHash("IsFly");
        _animatorParameterDictionary[EAnimatorParams.JUMP] = Animator.StringToHash("IsJump");
        _animatorParameterDictionary[EAnimatorParams.SIT] = Animator.StringToHash("IsSit");
        _animatorParameterDictionary[EAnimatorParams.SPIN] = Animator.StringToHash("IsSpin");

        // log
        Debug.Log($"AnimatorParameterDictionary has {_animatorParameterDictionary.Count} Elements");
    }

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    public static int GetAnimatorParameterHashCode(EAnimatorParams animatorParam)
    {
        if (_animatorParameterDictionary.TryGetValue(animatorParam, out var hashValue))
        {
            return hashValue;
        }

        throw new KeyNotFoundException("_animatorParameterDictionary's Key Not Found");
    }

    #endregion
}