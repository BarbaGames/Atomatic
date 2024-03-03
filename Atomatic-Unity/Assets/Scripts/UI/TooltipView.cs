using UnityEngine;
using TMPro;
using Generators;
using UnityEngine.UI;

namespace UI
{
    public class TooltipView : MonoBehaviour
    {
        [SerializeField] private TMP_Text description = null;
        [SerializeField] private TMP_Text production = null;
        [SerializeField] private TMP_Text unitEfficiency = null;
        [SerializeField] private TMP_Text totalGenerated = null;
        [SerializeField] private Image imgIcon = null;
        [SerializeField] private TextMeshProUGUI txtPrice = null;

        private GeneratorData selectedGen = null;
        
        public void OnTooltipEnable(GeneratorData generatorData)
        {
            selectedGen = generatorData;
            if (generatorData.showData || generatorData.unlocked)
            {
                description.text = generatorData.description;

                production.text = "Total E/s: " + System.Environment.NewLine + generatorData.currencyGenerated.ToString("N0");
                unitEfficiency.text = "Energy/lvl: " + System.Environment.NewLine + generatorData.baseCurrencyGenerated.ToString("N0");
                totalGenerated.text = "Generated: " + System.Environment.NewLine + generatorData.totalCurrencyGenerated.ToString("N0");
            }
            else
            {
                description.text = "???";
                production.text = "Total E/s: ";
                unitEfficiency.text = "Energy/lvl: ";
                totalGenerated.text = "Generated: ";
            }
        }

        public void OnTooltipEnable(Upgrade upgrade)
        {
            selectedGen = null;
            description.text = upgrade.description;
            imgIcon.sprite = upgrade.icon;
            imgIcon.enabled = true;
            txtPrice.text = "$" + upgrade.price.ToString();
            production.text = "";
            unitEfficiency.text = "Upgrade amount: " + System.Environment.NewLine + upgrade.currencyGeneratedAmount.ToString("N0");
            totalGenerated.text = "";
        }

        public void OnToolTipDisable()
        {
            selectedGen = null;
            imgIcon.enabled = false;
            txtPrice.text = "";
            description.text = "...";
            production.text = "";
            unitEfficiency.text = "";
            totalGenerated.text = "";
        }

        public void UpdateToolTip(GeneratorData generatorData)
        {
            if (selectedGen != null && generatorData.id == selectedGen.id)
            {
                OnTooltipEnable(generatorData);
            }
        }
    }
}