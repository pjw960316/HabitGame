using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartManagerMono : MonoBehaviour
{
    #region 1. Fields

    private List<IManager> _managers;
    private List<Type> _managerTypes;
    private const string MAIN_ASSEMBLY = "Assembly-CSharp";
    private const string MAIN_SCENE_NAME = "MainScene";

    #endregion

    #region 2. Properties

    //TODO : soundData를 얘가 들고 있는 이유는 Mono라서 그래.
    [SerializeField] private SoundData _soundData;
    public SoundData SoundData => _soundData;

    #endregion

    #region 3. Constructor

    private void Awake()
    {
        LoadInitialGameState();

        ChangeScene();
        
        //TODO : GameStartManagerMono를 Interface 기반으로 Dispose?
    }

    #endregion

    #region 4. Methods

    private void LoadInitialGameState()
    {
        LoadScriptableObjectAsset();

        CreateManagerClassByReflection();

        InjectScriptableObjectToManagers();
    }

    //TODO : Test Code임.
    private void LoadScriptableObjectAsset()
    {
        if (_soundData != null)
        {
            Debug.Log("not null");
        }
    }

    private void CreateManagerClassByReflection()
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
                    iManager.Init();
                }
            }
        }
    }

    // TODO : 중복 제거해 -> CallBack?
    // 흐름 상 CreateManagerClassByReflection 얘랑은 구분하는 게 맞음 iManager.Init() 바로 뒤에 쓰는 게 아니라.
    private void InjectScriptableObjectToManagers()
    {
        if (_managerTypes != null)
        {
            foreach (var type in _managerTypes)
            {
                var instance = Activator.CreateInstance(type);
                if (instance is IManager iManager)
                {
                    iManager.InitializeScriptableObject(_soundData); //TODO : 일단 테스트로 SoundData를 넘김
                }
            }
        }
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene(MAIN_SCENE_NAME);
    }

    #endregion


    //test nested Class
    [Serializable]
    private class ScriptableObjectLoader
    {
        [SerializeField] private SoundData _soundData;
        public SoundData SoundData => _soundData;
    }
}