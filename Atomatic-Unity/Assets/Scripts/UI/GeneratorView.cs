using System;
using Generators;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using Random = UnityEngine.Random;

namespace UI
{
    public class GeneratorView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TMP_Text counterTxt = null;
        [SerializeField] private Image background;
        private GeneratorData generatorData = null;
        private Action<GeneratorData> onEnableTooltip;
        private Action onDisableTooltip;
        private int scientistAmount = 0;
        private const int MaxScientistsAmount = 30;
        public string Id
        {
            get => generatorData.id;
        }

        public void Init(GeneratorData generatorData, Action<GeneratorData> onEnableTooltip, Action onDisableTooltip)
        {
            this.generatorData = generatorData;
            counterTxt.text = ""; 
            background.sprite = generatorData.background;
            this.onEnableTooltip = onEnableTooltip;
            this.onDisableTooltip = onDisableTooltip;
        }

        public void UpdateData(GeneratorData generatorData, GameObject[] scientist, bool fromSave)
        {
            this.generatorData = generatorData;

            int randomScientist = Random.Range(0, scientist.Length);
            if(scientistAmount >= MaxScientistsAmount) return;
            if (fromSave)
            {
                for (int i = 0; i < this.generatorData.level; i++)
                {
                    GameObject go = Instantiate(scientist[randomScientist], transform);
                    ScientistController scientistController = go.GetComponent<ScientistController>();
                    scientistController.transform.localPosition = new Vector3(-385, 20, 0);
                    scientistController.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    scientistController.StartYScaleLerp();
                }
            }
            else
            {
                GameObject go = Instantiate(scientist[randomScientist], transform);
                ScientistController scientistController = go.GetComponent<ScientistController>();
                scientistController.transform.localPosition = new Vector3(-385, 20, 0);
                scientistController.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                scientistController.StartYScaleLerp();
            }

            scientistAmount++;
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