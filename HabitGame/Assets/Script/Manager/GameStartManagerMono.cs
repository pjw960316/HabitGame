using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartManagerMono : MonoBehaviour
{
    private List<IManager> _managers;
	private const string MAIN_ASSEMBLY = "Assembly-CSharp";
    private const string MAIN_SCENE_NAME = "MainScene";
    
    private void Awake()
    {
        LoadInitialGameState();

        ChangeScene();
        //TODO : GameStartManagerMono를 Interface 기반으로 죽여.
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

    private void ChangeScene()
    {
        SceneManager.LoadScene(MAIN_SCENE_NAME);
    }
}
