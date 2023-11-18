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
        private GeneratorData generatorData = null;
        private Action<GeneratorData> onEnableTooltip;
        private Action onDisableTooltip;
        private GameObject scientist;

        public string Id
        {
            get => generatorData.id;
        }

        public void Init(GeneratorData generatorData, GameObject scientist, Action<GeneratorData> onEnableTooltip, Action onDisableTooltip)
        {
            this.scientist = scientist;
            this.generatorData = generatorData;
            counterTxt.text = ""; //generatorData.level.ToString();
            background.sprite = generatorData.background;
            this.onEnableTooltip = onEnableTooltip;
            this.onDisableTooltip = onDisableTooltip;
        }

        public void UpdateData(GeneratorData generatorData)
        {
            this.generatorData = generatorData;

            for (int i = 0; i < this.generatorData.level; i++)
            {
                GameObject go = Instantiate(scientist, transform);
                ScientistController scientistController = go.GetComponent<ScientistController>();
                scientistController.transform.localPosition = new Vector3(-385, 20, 0);
                scientistController.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                scientistController.StartYScaleLerp();
            }
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