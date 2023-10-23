using Generators;
using TMPro;

using UnityEngine;

namespace UI
{
    public class TooltipView : MonoBehaviour
    {
        [SerializeField] private TMP_Text infoText = null;
        [SerializeField] private Transform holder = null;
        
        public void OnTooltipEnable(GeneratorData generatorData)
        {
            infoText.text = "Generates " + generatorData.currencyGenerated + (generatorData.name == "generator 0" ? " per click" : " E/s") + " , Owned: " + generatorData.level + ", Cost: " + generatorData.levelUpCost;
            holder.gameObject.SetActive(true);
        }
        
        public void OnTooltipEnable(Upgrade upgrade)
        {
            infoText.text = upgrade.description + ", Cost: " + upgrade.price;
            holder.gameObject.SetActive(true);
        }

        public void OnToolTipDisable()
        {
            holder.gameObject.SetActive(false);
        }
    }
}