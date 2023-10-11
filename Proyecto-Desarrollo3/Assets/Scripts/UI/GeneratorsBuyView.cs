using System;
using System.Collections.Generic;
using Generators;
using UnityEngine;

namespace UI
{
    public class GeneratorsBuyView : MonoBehaviour
    {
        [SerializeField] private Transform scrollViewHolder = null;
        [SerializeField] private GameObject generatorViewPrefab = null;

        private List<GeneratorBuyView> generatorBuyViews = null;

        public void Init(List<GeneratorData> generatorStats, Action<string> onTryBuyGenerator, Action<GeneratorData> onEnableTooltip, Action onDisableTooltip)
        {
            generatorBuyViews = new List<GeneratorBuyView>();
            for (int i = 0; i < generatorStats.Count; i++)
            {
                GeneratorBuyView generatorBuyView = Instantiate(generatorViewPrefab, scrollViewHolder).GetComponent<GeneratorBuyView>();
                generatorBuyView.Init(generatorStats[i], onTryBuyGenerator, onEnableTooltip, onDisableTooltip);
                generatorBuyViews.Add(generatorBuyView);
            }
        }

        public void UpdateGenerator(GeneratorData generatorData)
        {
            for (int i = 0; i < generatorBuyViews.Count; i++)
            {
                if (generatorBuyViews[i].Id == generatorData.id)
                {
                    generatorBuyViews[i].UpdateData(generatorData);
                    return;
                }
            }
        }
    }
}