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
    private const int LOAD_SCENE_BACKGROUND_CHANGE_COUNT = 1;
#else
    private const int LOAD_SCENE_BACKGROUND_CHANGE_COUNT = 7;
#endif

    [SerializeField] private LoadBackgroundImageMono _loadBackgroundImageMono;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private List<ScriptableObject> _scriptableObjectModels;

    private Assembly _cSharpAssembly;

    private List<Type> _managerTypeList = new();
    private List<IManager> _managerList = new();

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    private void Awake()
    {
        Initialize();

        PreLoadAudioDataAsync().Forget();

        LiveGameStartManagerMonoPermanent();

        ShowGameLoadSceneBackgroundAsync().Forget();
    }

    private void Initialize()
    {
        InitializeManagerTypesByReflection();

        CreateManagers();
        
        // note : 이 시점 이후에는 싱글턴이 null이 되지 않음이 보장된다.

        InitializeManagers();
    }

    #endregion

    #region 3-1. InitializeManagers

    private void InitializeManagerTypesByReflection()
    {
        _cSharpAssembly = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(asm => asm.GetName().Name == MAIN_ASSEMBLY);
        if (_cSharpAssembly == null)
        {
            throw new NullReferenceException("_cSharpAssembly is null");
        }

        // note : ManagerBase<T>를 제외해야 한다. -> !type.IsAbstract
        _managerTypeList = _cSharpAssembly.GetTypes()
            .Where(type => typeof(IManager).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
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

    private void CreateManagers()
    {
        _managerList = new List<IManager>();

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

    private void InitializeManagers()
    {
        foreach (var manager in _managerList)
        {
            manager.PreInitialize();
        }

        InitializeModels();

        InjectModelsInManagers();

        foreach (var manager in _managerList)
        {
            manager.Initialize();
        }

        foreach (var manager in _managerList)
        {
            manager.BindEvent();
        }
    }

    #endregion

    #region 3-2. InitializeModels (ScriptableObject & XML)

    private void InitializeModels()
    {
        InitializeScriptableObjects();
        
        InitializeXmlData();
    }

    private void InjectModelsInManagers()
    {
        foreach (var manager in _managerList)
        {
            manager.SetModel();
        }
    }

    private void InitializeScriptableObjects()
    {
        var scriptableObjectList = new List<ScriptableObject>();
        var scriptableObjectCount = 0;
        
        foreach (var scriptableObject in _scriptableObjectModels)
        {
            scriptableObjectList.Add(scriptableObject);
            scriptableObjectCount++;
        }

        var scriptableObjectManager = ScriptableObjectManager.Instance;
        ExceptionHelper.CheckNullException(scriptableObjectManager, "scriptableObjectManager");
        
        ScriptableObjectManager.Instance.RegisterAllScriptableObjects(scriptableObjectList);

        //Log
        Debug.Log($"{scriptableObjectCount}개의 ScriptableObject가 Model로 _modelList에 추가되었습니다.");
    }

    private void InitializeXmlData()
    {
        var xmlDataManager = XmlDataManager.Instance;
        ExceptionHelper.CheckNullException(xmlDataManager, "xmlDataManager");

        xmlDataManager.RegisterDeserializedXmlData();

        //Log
        Debug.Log($"{xmlDataManager.GetDeserializedXmlListCount()}개의 xml이 Model로 _modelList에 추가되었습니다.");
    }

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    // 

    #endregion


    #region 6. Methods

    private void LiveGameStartManagerMonoPermanent()
    {
        //log
        Debug.Log("Live Permanent");

        DontDestroyOnLoad(this);
    }

    private void ChangeScene()
    {
        //log
        Debug.Log("Scene Change");

        SceneManager.LoadScene(MAIN_SCENE_NAME);
    }

    #endregion

    #region 7. Async Methods

    private async UniTaskVoid PreLoadAudioDataAsync()
    {
        //refactor
        var alarmData = ScriptableObjectManager.Instance.GetScriptableObject<AlarmData>();
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

        ChangeScene();
    }

    private async UniTaskVoid ShowGameLoadSceneBackgroundAsync()
    {
        var loadSceneBackgroundPlayTime =
            LOAD_SCENE_BACKGROUND_CHANGE_COUNT * _loadBackgroundImageMono.GetChangeBackgroundTime();

        await UniTask.Delay(TimeSpan.FromSeconds(loadSceneBackgroundPlayTime));

        Destroy(_loadBackgroundImageMono.gameObject);
        Destroy(_canvas);
    }

    #endregion
}