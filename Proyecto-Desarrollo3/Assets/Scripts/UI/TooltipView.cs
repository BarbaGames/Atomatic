using UnityEngine;

using TMPro;

using Generators;

using UnityEngine.UI;

namespace UI
{
    public class TooltipView : MonoBehaviour
    {
        [SerializeField] private TMP_Text description = null;
        [SerializeField] private Image imgIcon = null;
        [SerializeField] private TextMeshProUGUI txtPrice = null;

        public void OnTooltipEnable(GeneratorData generatorData)
        {
            if (generatorData.showData || generatorData.unlocked)
            {
                //title.text = generatorData.id;
                description.text = generatorData.description;
                //cost.text = "";

                //production.text = "Total E/s: " + generatorData.currencyGenerated.ToString("N0");
                //unitEfficiency.text = "Energy per unit: " + generatorData.baseCurrencyGenerated.ToString("N0");
            }
            else
            {
                //title.text = "???";
                description.text = "???";
                //cost.text = "";
                //production.text = "";
            }
        }

        public void OnTooltipEnable(Upgrade upgrade)
        {
            description.text = upgrade.description;
            imgIcon.sprite = upgrade.icon;
            imgIcon.enabled = true;
            txtPrice.text = "$" + upgrade.price.ToString();
            //cost.text = "Cost: " + upgrade.price;
            //production.text = "";
            //unitEfficiency.text = "Upgrade amount: " + upgrade.currencyGeneratedAmount;
        }

        public void OnToolTipDisable()
        {
            imgIcon.enabled = false;
            txtPrice.text = "";
            //title.text = "";
            description.text = "...";
            //cost.text = "";
            //production.text = "";
            //unitEfficiency.text = "";
        }
    }
}