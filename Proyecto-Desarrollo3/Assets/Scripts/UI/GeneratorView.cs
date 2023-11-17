using System;
using System.Collections.Generic;
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
        private List<ScientistController> scientists;
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
            scientists = new List<ScientistController>();
        }

        public void UpdateData(GeneratorData generatorData)
        {
            this.generatorData = generatorData;

            for (int i = 0; i < this.generatorData.level; i++)
            {
                ScientistController go = Instantiate(scientist, transform).GetComponent<ScientistController>();
                go.transform.localPosition = new Vector3(-385, 20, 0);
                go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                go.StartYScaleLerp();
                scientists.Add(go);
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