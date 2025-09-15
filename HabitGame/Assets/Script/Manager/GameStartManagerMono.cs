using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartManagerMono : MonoBehaviour
{
    #region 1. Fields

    private const string MAIN_ASSEMBLY = "Assembly-CSharp";
    private const string MAIN_SCENE_NAME = "MainScene";
    
#if UNITY_EDITOR
    private const int LOAD_SCENE_SHOW_TIME = 10; //ms
#else 
    private const int LOAD_SCENE_SHOW_TIME = 10000; 
#endif

    [SerializeField] private List<ScriptableObject> _scriptableObjectModels;

    private Assembly _cSharpAssembly;
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

        PreLoadAudioDataAsync().Forget();

        LiveGameStartManagerMonoPermanent();

        ShowGameLoadSceneAsync().Forget();
    }

    private void Initialize()
    {
        _managerList = new List<IManager>();
        _modelList = new List<IModel>();
    }

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    private void LoadInitialGameState()
    {
        //log
        Debug.Log("GameStartManagerMono : LoadInitialGameState Start");

        InitializeManagerTypesUsingReflection();

        InitializeManagers();

        InitializeModels();
    }

    private async UniTaskVoid PreLoadAudioDataAsync()
    {
        var alarmData = _modelList.OfType<AlarmData>().FirstOrDefault();
        if (alarmData == null)
        {
            throw new NullReferenceException("alarmData is null");
        }

        alarmData.Initialize();
        var sleepingAudioClipPathDictionary = alarmData.SleepingAudioClipPathDictionary;

        foreach (var element in sleepingAudioClipPathDictionary)
        {
            var key = element.Key;
            var relativePath = element.Value;

            var loadData = await Resources.LoadAsync<AudioClip>(relativePath);
            var memoryLoadedAudioClip = loadData as AudioClip;

            alarmData.SetSleepingAudioClipDictionary(key, memoryLoadedAudioClip);

            //log
            Debug.Log($"{relativePath}의 음원 파일 {memoryLoadedAudioClip?.name}이 비동기로 로드 되었습니다");
        }
    }

    private void InitializeManagerTypesUsingReflection()
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

        // Note
        // 싱글턴을 생성하고
        // 모든 세팅 전에 필요한 기능을 수행한다.
        foreach (var manager in _managerList)
        {
            manager.PreInitialize();
        }

        ConnectModelsInManagers();

        // Note
        // 모든 싱글턴이 생성된 이후의 시점이니
        // 참조 가능
        foreach (var manager in _managerList)
        {
            manager.Initialize();
        }
    }

    private void InitializeModels()
    {
        ModelManager.Instance.SetAllModels(_modelList);
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
        var scriptableObjectModelCount = 0;
        foreach (var scriptableObjectModel in _scriptableObjectModels)
        {
            if (scriptableObjectModel is IModel model)
            {
                _modelList.Add(model);
                scriptableObjectModelCount++;
            }
        }

        //Log
        Debug.Log($"{scriptableObjectModelCount}개의 ScriptableObject가 Model로 _modelList에 추가되었습니다.");
    }

    private void SetModelListWithDeserializedXml()
    {
        var _xmlDataSerializeManager = XmlDataSerializeManager.Instance;
        ExceptionHelper.CheckNullException(_xmlDataSerializeManager, "_xmlDataSerializeManager");

        var modelList = _xmlDataSerializeManager.GetModelListWithDeserializedXml();

        var xmlModelCount = 0;
        foreach (var model in modelList)
        {
            _modelList.Add(model);
            xmlModelCount++;
        }

        //Log
        Debug.Log($"{xmlModelCount}개의 xml이 Model로 _modelList에 추가되었습니다.");
    }

    private void LiveGameStartManagerMonoPermanent()
    {
        DontDestroyOnLoad(this);
    }

    private async UniTaskVoid ShowGameLoadSceneAsync()
    {
        await UniTask.Delay(LOAD_SCENE_SHOW_TIME);

        ChangeScene();
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene(MAIN_SCENE_NAME);
    }

    #endregion
}