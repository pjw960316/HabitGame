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

    [SerializeField] private List<ScriptableObject> _allModels;

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    private void Awake()
    {
        LoadInitialGameState();

        LivePermanent();

        ChangeScene();
    }

    #endregion

    #region 4. Methods

    private void LoadInitialGameState()
    {
        //Log
        Debug.Log("GameStartManagerMono  ->  LoadInitialGameState");

        CreateManagerInstances();
    }

    private void CreateManagerInstances()
    {
        var cSharpAssembly = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(asm => asm.GetName().Name == MAIN_ASSEMBLY);

        var managerTypes = cSharpAssembly?.GetTypes()
            .Where(type => typeof(IManager).IsAssignableFrom(type) && type.IsClass)
            .ToList();

        if (managerTypes != null)
        {
            foreach (var type in managerTypes)
            {
                object objectTypeInstance = Activator.CreateInstance(type);
               
                //test
                if (objectTypeInstance is SoundManager manager)
                {
                    manager.ConnectInstanceByActivator(manager);
                    manager.SetModel(_allModels);
                    manager.Initialize();
                }

                /*if (objectTypeInstance is SoundManager soundManager)
                {
                    SoundManager.Instance
                }*/
                /*if (instance is IManager manager)
                {
                    //test
                    manager = SoundManager.Instance;
                    
                    
                    manager.SetModel(_allModels);
                    manager.Initialize();
                }*/
            }
        }
        
        if (SoundManager.Instance.SoundData != null)
        {
            Debug.Log("111");
        }
    }

    


    //fix
    //현재 Scene이 변경 될 때 연결되어 있는 SO가 죽는 현상이 있는데
    //잠깐은 이 MONO를 살려서 유지시킨다.
    private void LivePermanent()
    {
        if (SoundManager.Instance.SoundData != null)
        {
            Debug.Log("22");
        }
        DontDestroyOnLoad(this);
        if (SoundManager.Instance.SoundData != null)
        {
            Debug.Log("33");
        }
    }
    private void ChangeScene()
    {
        if (SoundManager.Instance.SoundData != null)
        {
            Debug.Log("44");
        }
        SceneManager.LoadScene(MAIN_SCENE_NAME);
        if (SoundManager.Instance.SoundData != null)
        {
            Debug.Log("55");
        }
    }
    #endregion
}