using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * TODO:
 * GameStartMAnager에서 싱글턴을 모두 초기화 하는 걸 Awake랑 Start로 나누었다면
 * Awake와 Start에서 정확히 어떤 걸 초기화하면 좋을 지 연습해서 Personal Coding Style에 적자.
 *  예를 들면 Binding 같은건.
 */

public class GameStartManagerMono : MonoBehaviour
{
    private List<IManager> _managers;
    private void Awake()
    {
        LoadInitialGameState();
        
        //ConvertMainScene(); //TODO : 생성한 게 존재할 거야.
        //DontDestroyOnLoad(this); //TODO : 필요한 가?
    }
    
    private void ConvertMainScene()
    {
        var changeSceneManager = new ChangeSceneManager();
        changeSceneManager.ChangeScene();
    }

    //TODO : 이게 많아지면 private nested class로 분할 해서 각각 호출 되도록?
    private void LoadInitialGameState()
    {
        Debug.Log("hi");
        var cSharpAssembly = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(asm => asm.GetName().Name == "Assembly-CSharp");
        
        var managerTypes = cSharpAssembly?.GetTypes()
            .Where(type => typeof(IManager).IsAssignableFrom(type) && type.IsClass)
            .ToList();

        foreach (var type in managerTypes)
        {
            var tmp = Activator.CreateInstance(type);
            var tmp2 = tmp as IManager;
            
            tmp2.Init();
        }
    }
}


public class ChangeSceneManager
{
    public void ChangeScene()
    {
        SceneManager.LoadScene("Main");
    }
}