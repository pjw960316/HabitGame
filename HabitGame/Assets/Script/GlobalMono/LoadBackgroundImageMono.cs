using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

// note
// LoadScene에서만 사용할 스크립트
public class LoadBackgroundImageMono : MonoBehaviour
{
    #region 1. Fields

    private const int CHANGE_BACKGROUND_TIME = 1;
    
    [SerializeField] private Image _backgroundSprite;
    [SerializeField] private List<Sprite> _backgroundSpriteList = new();

    private int _listIndex;
    private int _listMaxIndex;

    private IDisposable _changeBackgroundSpriteDisposable;

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    private void Awake()
    {
        _listIndex = 0;
        _listMaxIndex = _backgroundSpriteList.Count - 1;

        BindEvent();
    }

    private void BindEvent()
    {
        _changeBackgroundSpriteDisposable =
            Observable.Interval(TimeSpan.FromSeconds(CHANGE_BACKGROUND_TIME))
                .Subscribe(_ => OnChangeBackgroundSprite());
    }

    #endregion

    #region 4. EventHandlers

    private void OnChangeBackgroundSprite()
    {
        _listIndex++;

        if (_listMaxIndex < _listIndex)
        {
            _listIndex = 0;
        }

        _backgroundSprite.sprite = _backgroundSpriteList[_listIndex];
    }

    #endregion

    #region 5. Request Methods

    // 

    #endregion

    #region 6. Methods

    private void OnDestroy()
    {
        _backgroundSpriteList.Clear();
        _backgroundSpriteList = null;

        _changeBackgroundSpriteDisposable.Dispose();
    }

    #endregion
}