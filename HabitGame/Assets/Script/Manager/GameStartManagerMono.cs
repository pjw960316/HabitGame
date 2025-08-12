using System;
using System.Collections.Generic;
using System.IO;
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
        //log
        Debug.Log("GameStartManagerMono LoadInitialGameState()");
        
        SetManagerTypesUsingReflection();

        InitializeManagers();
        
        //test
        XmlDataSerializeManager.Instance.Test();
        

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

        // Note
        // 모든 싱글턴이 생성된 이후의 시점이니
        // 참조 가능
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
                //이거 개수 검사 로직
                _modelList.Add(model);
            }
        }
    }

    private void SetModelListWithDeserializedXml()
    {
        var dataManager = XmlDataSerializeManager.Instance;
        
        ExceptionHelper.CheckNullException(dataManager, "DataManager");
        
        // todo
        // 모든 XML Deserialize data를 manager에 연결
        //test
        
        //var path = Path.Combine(Application.dataPath, "Resources/MyCharacterData");
        
        //test path
        var path = "C:/HabitGame/HabitGame/Assets/Resources/MyCharacterData.xml";
        
        var test = dataManager.GetDeserializedXmlData<MyCharacterData>(path);
        _modelList.Add(test);
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