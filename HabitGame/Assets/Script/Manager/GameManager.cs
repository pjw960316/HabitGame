using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

//note : 책임
// Manager 들 관리
public class GameManager : ManagerBase<GameManager>
{
    #region 1. Fields

    private readonly Dictionary<Type, IManager> _managerDict = new();

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    public sealed override void Initialize()
    {
        TurnOnScreenAlways();
    }

    #endregion

    #region 4. Methods

    private void TurnOnScreenAlways()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public void SetManagers(List<IManager> managerList)
    {
        foreach (var runTimeManager in managerList)
        {
            _managerDict[runTimeManager.GetType()] = runTimeManager;
        }
    }

    public int GetManagersCount()
    {
        return _managerDict.Count;
    }

    public TManager GetManagerByType<TManager>() 
    where TManager : class, IManager
    {
        if (_managerDict.TryGetValue(typeof(TManager), out var manager))
        {
            return manager as TManager;
        }
        throw new KeyNotFoundException($"Manager not found: {typeof(TManager)}");
    }
    
    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}