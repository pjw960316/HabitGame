using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartManagerMono : MonoBehaviour
{
    #region 1. Fields

    private const string MAIN_ASSEMBLY = "Assembly-CSharp";
    private const string MAIN_SCENE_NAME = "MainScene";

    [SerializeField] private List<ScriptableObject> _scriptableObjectModels;
    
    private Assembly _cSharpAssembly;
    
    // NOTE
    // ScriptableObject + XML Deserialized Model
    private List<Type> _managerTypeList;
    private List<IManager> _managerList;
    private List<IModel> _modelList;

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    private void Awake()
    {
        Initialize();
        
        LoadInitialGameState();
    }

    private void Initialize()
    {
        _modelList = new List<IModel>();
        _managerList = new List<IManager>();
    }

    #endregion

    #region 4. Methods

    private void LoadInitialGameState()
    {
        SetManagerTypesUsingReflection();

        InitializeManagers();

        LivePermanent();

        ChangeScene();
    }

    private void SetManagerTypesUsingReflection()
    {
        _cSharpAssembly = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(asm => asm.GetName().Name == MAIN_ASSEMBLY);

        if (_cSharpAssembly == null)
        {
            throw new NullReferenceException("_cSharpAssembly is null");
        }

        _managerTypeList = _cSharpAssembly.GetTypes()
            .Where(type => typeof(IManager).IsAssignableFrom(type) && type.IsClass)
            .ToList();

        if (_managerTypeList == null)
        {
            throw new NullReferenceException("_managerTypeList is null");
        }

        if (_managerTypeList.Count == 0)
        {
            throw new ArgumentOutOfRangeException();
        }
    }


    private void InitializeManagers()
    {
        CreateSingletonManagers();

        ConnectModelsInManagers();

        foreach (var manager in _managerList)
        {
            manager.Initialize();
        }
    }

    // Note
    // 여기서 각각의 생성자를 호출하며 Type에 맞는 Instance를 생성한다.
    // 그리고 연결을 ConnectInstanceByActivator
    private void CreateSingletonManagers()
    {
        foreach (var type in _managerTypeList)
        {
            var objectTypeInstance = Activator.CreateInstance(type);

            if (objectTypeInstance is IManager manager)
            {
                manager.ConnectInstanceByActivator(manager);
                _managerList.Add(manager);
            }
        }
    }

    // refactor
    // modellist를 readonly로 전달하면 좋을듯?
    private void ConnectModelsInManagers()
    {
        SetModelList();
        
        foreach (var manager in _managerList)
        {
            manager.SetModel(_modelList);
        }
    }

    private void SetModelList()
    {
        SetModelListWithScriptableObject();
        SetModelListWithDeserializedXml();
    }

    private void SetModelListWithScriptableObject()
    {
        foreach (var scriptableObjectModel in _scriptableObjectModels)
        {
            if (scriptableObjectModel is IModel model)
            {
                //todo
                //이거 실수로 빠지면 안 됨.
                _modelList.Add(model);
            }
        }
    }

    private void SetModelListWithDeserializedXml()
    {
        //todo
        //이거 실수로 빠지면 안 됨.
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