using BarbaGames.Game.Generators;

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
            infoText.text = generatorData.id + ", Owned: " + generatorData.level + ", Cost: " + generatorData.levelUpCost;
            holder.gameObject.SetActive(true);
        }

        public void OnToolTipDisable()
        {
            holder.gameObject.SetActive(false);
        }
    }
}