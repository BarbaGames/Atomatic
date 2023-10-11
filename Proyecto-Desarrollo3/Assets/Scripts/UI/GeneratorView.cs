using System;
using Generators;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class GeneratorView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TMP_Text counterTxt = null;
        private GeneratorData generatorData = null;
        private Action<GeneratorData> onEnableTooltip;
        private Action onDisableTooltip;
        public string Id { get => generatorData.id; }

        public void Init(GeneratorData generatorData, Action<GeneratorData> onEnableTooltip, Action onDisableTooltip)
        {
            this.generatorData = generatorData;
            counterTxt.text = generatorData.level.ToString();
            this.onEnableTooltip = onEnableTooltip;
            this.onDisableTooltip = onDisableTooltip;
        }

        public void UpdateData(GeneratorData generatorData)
        {
            this.generatorData = generatorData;
            counterTxt.text = generatorData.level.ToString();
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
