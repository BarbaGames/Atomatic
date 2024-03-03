using System;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeButtonView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Button btnBuy = null;
    [SerializeField] private Image imgIcon = null;

    private int id;
    private Action<Upgrade> onEnableTooltip;
    private Action onDisableTooltip;
    private Upgrade upgradeData;
    
    public int Id { get => id; }

    public void Init(Upgrade upgradeData, Action<int> onTryBuyUpgrade, Action<Upgrade> onEnableTooltip, Action onDisableTooltip, bool empty = false)
    {
        if (empty)
        {
            imgIcon.enabled = false;
            return;
        }
        this.upgradeData = upgradeData;
        imgIcon.sprite = this.upgradeData.icon;
        id = this.upgradeData.id;
        this.onEnableTooltip += onEnableTooltip;
        this.onDisableTooltip += onDisableTooltip;
        
        btnBuy.onClick.AddListener( () =>
        {
            onTryBuyUpgrade.Invoke(this.id);
        });

        if (upgradeData.bought)
        {
            btnBuy.interactable = false;
        }
    }

    public void UpdateUpgrade(Upgrade upgradeData)
    {
        this.upgradeData = upgradeData;
        
        if (upgradeData.bought)
        {
            btnBuy.interactable = false;
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        onEnableTooltip.Invoke(upgradeData);
    }
        
    public void OnPointerExit(PointerEventData eventData)
    {
        onDisableTooltip.Invoke();
    }
}
