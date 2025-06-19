using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartManagerMono : MonoBehaviour
{
    private List<IManager> _managers;
	private const string MAIN_ASSEMBLY = "Assembly-CSharp";
    private void Awake()
    {
        LoadInitialGameState();

        //ConvertMainScene(); //TODO : 생성한 게 존재할 거야.
        //DontDestroyOnLoad(this); //TODO : 필요한 가?
    }

    private void LoadInitialGameState()
    {
        CreateManagerClassByReflection();
    }

    private void CreateManagerClassByReflection()
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
                var instance = Activator.CreateInstance(type);
                if (instance is IManager iManager)
                {
                    iManager.Init();
                }
            }
        }
    }

    private void ConvertMainScene()
    {
        var changeSceneManager = new ChangeSceneManager();
        changeSceneManager.ChangeScene();
    }
}

public class ChangeSceneManager
{
    public void ChangeScene()
    {
        SceneManager.LoadScene("Main");
    }
}