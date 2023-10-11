using System;
using System.Collections.Generic;

using UnityEngine;

public class UpgradeBuyView : MonoBehaviour
{
    [SerializeField] private Transform scrollViewHolder = null;
    [SerializeField] private GameObject upgradeViewPrefab = null;

    private List<UpgradeButtonView> upgradeButtonViews = null;

    public void Init(List<Upgrade> upgrades, Action<int> onTryBuyUpgrade, Action<Upgrade> onEnableTooltip, Action onDisableTooltip)
    {
        upgradeButtonViews = new List<UpgradeButtonView>();
        for (int i = 0; i < upgrades.Count; i++)
        {
            UpgradeButtonView upgradeButtonView = Instantiate(upgradeViewPrefab, scrollViewHolder).GetComponent<UpgradeButtonView>();
            upgradeButtonView.Init(upgrades[i], onTryBuyUpgrade, onEnableTooltip, onDisableTooltip);
            upgradeButtonViews.Add(upgradeButtonView);
        }
    }

    public void UpdateUpgrade(Upgrade upgrade)
    {
        for (int i = 0; i < upgradeButtonViews.Count; i++)
        {
            if (upgradeButtonViews[i].Id == upgrade.id)
            {
                upgradeButtonViews[i].UpdateUpgrade(upgrade);
                return;
            }
        }
    }
}