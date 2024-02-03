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
        [SerializeField] private TMP_Text cost = null;
        [SerializeField] private TMP_Text production = null;
        [SerializeField] private TMP_Text unitEfficiency = null;
        [SerializeField] private Transform holder = null;

        public void OnTooltipEnable(GeneratorData generatorData)
        {
            if (generatorData.showData || generatorData.unlocked) 
            {
                title.text = generatorData.id;
                description.text = generatorData.description;
                cost.text = "";
                
                production.text = "Total E/s: " + generatorData.currencyGenerated.ToString("N0");
                unitEfficiency.text = "Energy per unit: " + generatorData.baseCurrencyGenerated.ToString("N0");
                //historicalProduction.text = "Total generated: " + generatorData.totalCurrencyGenerated.ToString("N0");
            }
            else
            {
                title.text = "???";
                description.text = "";
                cost.text = "";
                production.text = "";
                //unitEfficiency.text = "";
                //historicalProduction.text = "";
            }
            holder.gameObject.SetActive(true);
        }

        public void OnTooltipEnable(Upgrade upgrade)
        {
            description.text = upgrade.description;
            cost.text = "Cost: " + upgrade.price;
            production.text = "";
            unitEfficiency.text = "Upgrade amount: " + upgrade.currencyGeneratedAmount;
            //historicalProduction.text = "";
            holder.gameObject.SetActive(true);
        }

        public void OnToolTipDisable()
        {
            holder.gameObject.SetActive(false);
        }
    }
}