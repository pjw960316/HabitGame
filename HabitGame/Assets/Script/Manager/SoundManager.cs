using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[Serializable]
public class SoundManager : ManagerBase<SoundManager>, IManager, IDisposable
{
    #region 1. Fields

    private AudioSource _audioSource;
    private CompositeDisposable _disposable = new();
    public Subject<Unit> TestEvent;

    #endregion

    #region 2. Properties

    private SoundData _soundData;

    public SoundData SoundData
    {
        get => _soundData;
        private set => _soundData = value;
    }

    #endregion

    #region 3. Constructor

    public SoundManager()
    {
        // LOG
        //Debug.Log("SoundManager Constructor");

        TestEvent = new Subject<Unit>();
    }

    #endregion

    #region 4. Methods

    public void Initialize()
    {
        BindEvent();
    }

    private void BindEvent()
    {
        // test
        TestEvent.Subscribe(_ => TestPlayMusic()).AddTo(_disposable);
    }

    // Test
    private void TestPlayMusic()
    {
        _audioSource?.Play();
    }


    public void SetModel(IEnumerable<ScriptableObject> models)
    {
        foreach (var model in models)
        {
            if (model is SoundData soundData)
            {
                SoundData = soundData;
                return;
            }
        }
    }
    
    public IPresenter GetPresenterAfterCreate<TPresenter>(IView view) where TPresenter : IPresenter, new()
    {
        TPresenter presenter = new TPresenter();
        presenter.Initialize(view);
        
        return presenter;
    }


    public void InitializeScriptableObject(IModel data)
    {
        if (data is SoundData soundData)
        {
            _soundData = soundData;
        }
    }

    public void ConnectInstanceByActivator(IManager instance)
    {
        if (_instance == null)
        {
            _instance = instance as SoundManager;
        }
    }

    public void SetAudioSource(AudioSource audioSource)
    {
        _audioSource = audioSource;
    }

    public void SetAudioClip(AudioClip audioClip)
    {
        _audioSource.clip = audioClip;
    }

    public void Dispose()
    {
    }

    #endregion
}