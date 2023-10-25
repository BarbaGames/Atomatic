using System;
using Generators;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class GeneratorView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TMP_Text counterTxt = null;
        [SerializeField] private Image background;
        private GameObject scientist;
        private GeneratorData generatorData = null;
        private Action<GeneratorData> onEnableTooltip;
        private Action onDisableTooltip;
        public string Id { get => generatorData.id; }

        public void Init(GeneratorData generatorData, GameObject scientist, Action<GeneratorData> onEnableTooltip, Action onDisableTooltip)
        {
            this.generatorData = generatorData;
            counterTxt.text = generatorData.level.ToString();
            background.sprite = generatorData.background;
            this.scientist = scientist;
            this.onEnableTooltip = onEnableTooltip;
            this.onDisableTooltip = onDisableTooltip;
            // Test
            GameObject testScientist = Instantiate(this.scientist, transform);
            testScientist.transform.position = transform.position;
            //testScientist.transform.position.x += transform.position.
        }

        public void UpdateData(GeneratorData generatorData)
        {
            this.generatorData = generatorData;
            counterTxt.text = generatorData.level.ToString();
            //instanciate new scientist per level
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
