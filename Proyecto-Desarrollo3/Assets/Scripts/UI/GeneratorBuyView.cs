using System;
using System.Globalization;
using Generators;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class GeneratorBuyView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Button btnBuy = null;
        [SerializeField] private TMP_Text txtName = null;
        [SerializeField] private TMP_Text txtPrice = null;
        [SerializeField] private Image imgIcon = null;

        private string id = null;
        private Action<GeneratorData> onEnableTooltip;
        private Action onDisableTooltip;
        private GeneratorData generatorData;
    
        public string Id { get => id; }

        public void Init(GeneratorData generatorData, Action<string> onTryBuyGenerator, Action<GeneratorData> onEnableTooltip, Action onDisableTooltip)
        {
            this.generatorData = generatorData;
            imgIcon.sprite = this.generatorData.icon;
            id = this.generatorData.id;
            txtName.text = this.generatorData.id;
            txtPrice.text = this.generatorData.levelUpCost.ToString(CultureInfo.InvariantCulture);
            this.onEnableTooltip = onEnableTooltip;
            this.onDisableTooltip = onDisableTooltip;
        
            btnBuy.onClick.AddListener( () =>
            {
                onTryBuyGenerator.Invoke(this.id);
            });
        }

        public void UpdateData(GeneratorData generatorData)
        {
            this.generatorData = generatorData;
            txtPrice.text = generatorData.levelUpCost.ToString(CultureInfo.InvariantCulture);
            onEnableTooltip.Invoke(this.generatorData);
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            onEnableTooltip.Invoke(generatorData);
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            onDisableTooltip.Invoke();
        }
    }
}