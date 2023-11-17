using System;
using System.Collections.Generic;
using Generators;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI
{
    public class GeneratorsView : MonoBehaviour
    {
        [SerializeField] private GameObject generatorViewGo = null;
        [SerializeField] private Transform scrollViewHolder = null;
        [SerializeField] private Scrollbar scrollbar = null;
        [SerializeField] private GameObject[] scientistsPrefab = null;
        private List<GeneratorView> generatorViews = null;

        private void Update()
        {
            scrollbar.size = 0.1f;
        }

        public void Init()
        {
            generatorViews = new List<GeneratorView>();
        }

        public void AddGenerator(GeneratorData genData, Action<GeneratorData> onEnableTooltip, Action onDisableTooltip)
        {
            GeneratorView generatorView = Instantiate(generatorViewGo, scrollViewHolder).GetComponent<GeneratorView>();
            int randomScientist = Random.Range(0, scientistsPrefab.Length);
            generatorView.Init(genData, scientistsPrefab[randomScientist], onEnableTooltip, onDisableTooltip);
            generatorViews.Add(generatorView);
        }

        public void UpdateGenerator(GeneratorData generatorData)
        {
            for (int i = 0; i < generatorViews.Count; i++)
            {
                if (generatorViews[i].Id == generatorData.id)
                {
                    generatorViews[i].UpdateData(generatorData);
                    return;
                }
            }
        }
    }
}