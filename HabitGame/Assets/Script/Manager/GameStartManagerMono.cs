using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartManagerMono : MonoBehaviour
{
    #region 1. Fields

    private const string MAIN_ASSEMBLY = "Assembly-CSharp";
    private const string MAIN_SCENE_NAME = "MainScene";

    private List<IManager> _managers;
    private List<Type> _managerTypes;

    [SerializeField] private List<ScriptableObject> _allModels;

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    private void Awake()
    {
        LoadInitialGameState();

        ChangeScene();
    }

    #endregion

    #region 4. Methods

    private void LoadInitialGameState()
    {
        CreateManagerInstances();
    }

    private void CreateManagerInstances()
    {
        var cSharpAssembly = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(asm => asm.GetName().Name == MAIN_ASSEMBLY);

        _managerTypes = cSharpAssembly?.GetTypes()
            .Where(type => typeof(IManager).IsAssignableFrom(type) && type.IsClass)
            .ToList();
        
        if (_managerTypes != null)
        {
            foreach (var type in _managerTypes)
            {
                var instance = Activator.CreateInstance(type);
                if (instance is IManager iManager)
                {
                    iManager.ConnectInstanceByActivator(iManager);
                    iManager.SetModel(_allModels);
                    iManager.Init();
                }
            }
        }
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene(MAIN_SCENE_NAME);
    }

    #endregion


    /*
     * Test
     * nested Class
     */
    [Serializable]
    private class ScriptableObjectLoader
    {
        [SerializeField] private SoundData _soundData;
        public SoundData SoundData => _soundData;
    }
}