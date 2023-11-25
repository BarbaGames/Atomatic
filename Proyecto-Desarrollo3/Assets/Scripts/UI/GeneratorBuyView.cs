using System;
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
        [SerializeField] private TMP_Text txtOwned = null;
        [SerializeField] private Image imgIcon = null;
        [SerializeField] private Image imgIconShadow = null;

        private int id = -1;
        private Action<GeneratorData> onEnableTooltip;
        private Action onDisableTooltip;
        private GeneratorData generatorData;
    
        public int Id { get => id; }

        public void Init(GeneratorData generatorData, Action<int> onTryBuyGenerator, Action<GeneratorData> onEnableTooltip, Action onDisableTooltip)
        {
            this.generatorData = generatorData;
            imgIcon.sprite = this.generatorData.icon;
            imgIcon.enabled = generatorData.unlocked;
            imgIconShadow.sprite = this.generatorData.icon;
            id = this.generatorData.numId;
            txtName.text = generatorData.unlocked ? this.generatorData.id : "???";
            txtPrice.text = this.generatorData.levelUpCost.ToString("N0");
            txtOwned.text = this.generatorData.level.ToString("N0");
            this.onEnableTooltip = onEnableTooltip;
            this.onDisableTooltip = onDisableTooltip;
        
            btnBuy.onClick.AddListener( () =>
            {
                onTryBuyGenerator.Invoke(this.id);
            });
        }

        public void OnUpdateEnergy(long newEnergy)
        {
            if (newEnergy >= generatorData.levelUpCost)
            {
                generatorData.showData = true;
                imgIcon.enabled = true;
                txtName.text = generatorData.id;
                txtPrice.color = Color.white;
            }
            else
            {
                generatorData.showData = false;
                txtPrice.color = Color.red;
            }
        }
        
        public void UpdateData(GeneratorData generatorData)
        {
            this.generatorData = generatorData;
            txtPrice.text = generatorData.levelUpCost.ToString("N0");
            txtOwned.text = generatorData.level.ToString("N0");
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