using System;
using TMPro;
using UniRx;
using UnityEngine;

public class UIConfirmPopup : UIPopupBase
{
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private UIButtonBase _confirmButton;

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
        _confirmButton.Button.OnClickAsObservable()
            .Subscribe(_ => _onConfirmed.OnNext(Unit.Default))
            .AddTo(_disposables);
    }

    private void OnDestroy()
    {
        _disposables.Dispose();
        _onConfirmed.OnCompleted();
        _onConfirmed.Dispose();
    }
}
