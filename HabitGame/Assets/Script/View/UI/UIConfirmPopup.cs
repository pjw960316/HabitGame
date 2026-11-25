using System;
using TMPro;
using UniRx;
using UnityEngine;

public class UIConfirmPopup : UIPopupBase
{
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private UIButtonBase _confirmButton;

    // NOTE
    // 모든 ConfirmPopup의 메인 책임은
    // 확인을 눌렀을 때 Subscribe에 걸린 메서드의 실행이다. => RX
    private readonly Subject<Unit> _onConfirmed = new();
    public IObservable<Unit> OnConfirmed => _onConfirmed;

    protected override void InitializeEPopupKey()
    {
        _ePopupKey = EPopupKey.ConfirmPopup;
    }

    protected override void CreatePresenterByManager()
    {
    }

    protected override void BindEvent()
    {
        _confirmButton.OnClick.AddListener(() => _onConfirmed.OnNext(Unit.Default));
    }

    private void OnDestroy()
    {
        _disposables.Dispose();
        _onConfirmed.OnCompleted();
        _onConfirmed.Dispose();
    }
}
