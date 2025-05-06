using UnityEngine;

public class UIEarnMoneyBtn : UIButton
{
    private void Start()
    {
        button.onClick.AddListener(EarnMoney); //button에 ?를 붙여야 하는가
    }

    private void EarnMoney()
    {
    }
}