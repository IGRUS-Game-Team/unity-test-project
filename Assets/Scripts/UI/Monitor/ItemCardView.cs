using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemCardView : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text unitPriceText;
    [SerializeField] TMP_Text amountText;
    [SerializeField] TMP_Text totalText;
    [SerializeField] Button plusBtn, minusBtn, addBtn;

    private ItemData data;
    private int amount = 1;

    public void Setup(ItemData item, Action<ItemData,int> onAddToCart)
    {
        data = item;
        icon.sprite = data.icon;
        nameText.text = data.displayName;
        unitPriceText.text = $"${data.unitPrice:F2}";
        UpdateAmount(amount);

        plusBtn.onClick.AddListener(()=>ChangeAmount(1));
        minusBtn.onClick.AddListener(()=>ChangeAmount(-1));
        addBtn.onClick.AddListener(()=> onAddToCart?.Invoke(data, amount));
    }

    void ChangeAmount(int delta)
    {
        amount = Mathf.Clamp(amount + delta, 1, 999);
        UpdateAmount(amount);
    }

    void UpdateAmount(int a)
    {
        amountText.text = a.ToString();
        totalText.text  = $"${(data.unitPrice * a):F2}";
    }
}
