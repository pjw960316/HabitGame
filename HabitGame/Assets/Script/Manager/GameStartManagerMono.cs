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
                // Note
                // 여기서 각각의 생성자를 호출하며 Type에 맞는 Instance를 생성한다.
                // 그리고 연결을 ConnectInstanceByActivator
                var objectTypeInstance = Activator.CreateInstance(type);

                if (objectTypeInstance is IManager manager)
                {
                    manager.ConnectInstanceByActivator(manager);
                    manager.SetModel(_allModels);
                    manager.Initialize();
                }
            }
        }
    }

    //fix
    //현재 Scene이 변경 될 때 연결되어 있는 SO가 죽는 현상이 있는데
    //잠깐은 이 MONO를 살려서 유지시킨다.
    private void LivePermanent()
    {
        DontDestroyOnLoad(this);
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene(MAIN_SCENE_NAME);
    }

    #endregion
}