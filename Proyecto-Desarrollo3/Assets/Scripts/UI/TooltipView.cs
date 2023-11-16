using Generators;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class TooltipView : MonoBehaviour
    {
        [SerializeField] private TMP_Text title = null;
        [SerializeField] private TMP_Text description = null;
        [SerializeField] private TMP_Text quantity = null;
        [SerializeField] private TMP_Text cost = null;
        [SerializeField] private TMP_Text production = null;
        [SerializeField] private TMP_Text unitEfficiency = null;
        [SerializeField] private TMP_Text historicalProduction = null;
        [SerializeField] private Transform holder = null;

        public void OnTooltipEnable(GeneratorData generatorData)
        {
            //title.text = "Generates " + generatorData.currencyGenerated + (generatorData.name == "generator 0" ? " per click" : " E/s") + " , Owned: " + generatorData.level + ", Cost: " + generatorData.levelUpCost;
            title.text = generatorData.id;
            description.text = generatorData.description;
            quantity.text = "Owned: " + generatorData.level;
            cost.text = "Cost: " + generatorData.levelUpCost.ToString("N0");
            production.text = "Total E/s: " + generatorData.currencyGenerated.ToString("N0");
            unitEfficiency.text = "Energy per unit: " + generatorData.baseCurrencyGenerated.ToString("N0");
            historicalProduction.text = "Total generated: " + generatorData.totalCurrencyGenerated.ToString("N0");
            holder.gameObject.SetActive(true);
        }

        public void OnTooltipEnable(Upgrade upgrade)
        {
            description.text = upgrade.description;
            cost.text = "Cost: " + upgrade.price;
            quantity.text = " ";
            cost.text = " ";
            production.text = " ";
            unitEfficiency.text = " ";
            historicalProduction.text = " ";
            holder.gameObject.SetActive(true);
        }

        public void OnToolTipDisable()
        {
            holder.gameObject.SetActive(false);
        }
    }
}