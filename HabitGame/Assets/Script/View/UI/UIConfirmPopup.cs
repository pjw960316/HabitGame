using TMPro;
using UnityEngine;

public class UIConfirmPopup : UIPopupBase
{
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private UIButtonBase _confirmButton;

    protected override void InitializeEPopupKey()
    {
        _ePopupKey = EPopupKey.ConfirmPopup;
    }

    protected override void CreatePresenterByManager()
    {
    }

    protected override void BindEvent()
    {
    }
}
