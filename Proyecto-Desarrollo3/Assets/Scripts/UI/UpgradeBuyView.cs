using System;
using System.Collections.Generic;

using Generators;

using UnityEngine;
using UnityEngine.UI;

public class UpgradeBuyView : MonoBehaviour
{
    [SerializeField] private Transform scrollViewHolder = null;
    [SerializeField] private GameObject upgradeViewPrefab = null;
    [SerializeField] private Scrollbar scrollbar = null;
    
    private List<UpgradeButtonView> upgradeButtonViews = null;
    private List<Upgrade> upgrades = null;
    private Action<int> onTryBuyUpgrade;
    private Action<Upgrade> onEnableTooltip;
    private Action onDisableTooltip;

    private void Update()
    {
        scrollbar.size = 0.1f;
    }
    
    public void Init(List<Upgrade> upgrades, Action<int> onTryBuyUpgrade, Action<Upgrade> onEnableTooltip, Action onDisableTooltip)
    {
        this.upgrades = upgrades;
        this.onTryBuyUpgrade = onTryBuyUpgrade;
        this.onEnableTooltip = onEnableTooltip;
        this.onDisableTooltip = onDisableTooltip;
        upgradeButtonViews = new List<UpgradeButtonView>();
        for (int i = 0; i < 1; i++)
        {
            UpgradeButtonView upgradeButtonView = Instantiate(upgradeViewPrefab, scrollViewHolder).GetComponent<UpgradeButtonView>();
            upgradeButtonView.Init(upgrades[i], onTryBuyUpgrade, onEnableTooltip, onDisableTooltip);
            upgradeButtonViews.Add(upgradeButtonView);
        }
    }

    public void UpdateUpgrade(Upgrade upgrade, GeneratorData generatorData)
    {
        for (int i = 0; i < upgradeButtonViews.Count; i++)
        {
            if (upgradeButtonViews[i].Id == upgrade.id)
            {
                upgradeButtonViews[i].UpdateUpgrade(upgrade, generatorData);
                return;
            }
        }
    }

    public void UnlockUpgrade(Upgrade upgrade)
    {
        int nextId = upgrade.id + 1;
        if (nextId < upgrades.Count && (upgrade.generatorId == upgrades[nextId].generatorId || upgrades[nextId].stageUpgrade)) 
        {
            UpgradeButtonView upgradeButtonView = Instantiate(upgradeViewPrefab, scrollViewHolder).GetComponent<UpgradeButtonView>();
            upgradeButtonView.Init(upgrades[nextId], onTryBuyUpgrade, onEnableTooltip, onDisableTooltip);
            upgradeButtonViews.Add(upgradeButtonView);
        }
    }
    
    public void UnlockUpgrade(GeneratorData generatorData)
    {
        for (int i = 0; i < upgrades.Count; i++)
        {
            if (upgrades[i].generatorId == generatorData.numId)
            {
                UpgradeButtonView upgradeButtonView = Instantiate(upgradeViewPrefab, scrollViewHolder).GetComponent<UpgradeButtonView>();
                upgradeButtonView.Init(upgrades[i], onTryBuyUpgrade, onEnableTooltip, onDisableTooltip);
                upgradeButtonViews.Add(upgradeButtonView);
                return;
            }
        }
    }
}