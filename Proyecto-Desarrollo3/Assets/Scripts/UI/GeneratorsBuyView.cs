using System;
using System.Collections.Generic;

using Generators;

using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GeneratorsBuyView : MonoBehaviour
    {
        [SerializeField] private Transform scrollViewHolder = null;
        [SerializeField] private GameObject generatorViewPrefab = null;
        [SerializeField] private Scrollbar scrollbar = null;
        
        private List<GeneratorBuyView> generatorBuyViews = null;
        private Action<int> onTryBuyGenerator;
        private Action<GeneratorData> onEnableTooltip;
        private Action onDisableTooltip;

        private void Update()
        {
            scrollbar.size = 0.1f;
        }

        public void Init(List<GeneratorData> generatorStats, Action<int> onTryBuyGenerator, Action<GeneratorData> onEnableTooltip, Action onDisableTooltip, bool newGame)
        {
            this.onTryBuyGenerator = onTryBuyGenerator;
            this.onEnableTooltip = onEnableTooltip;
            this.onDisableTooltip = onDisableTooltip;
            
            generatorBuyViews = new List<GeneratorBuyView>();
            if (newGame)
            {
                for (int i = 0; i < 2; i++)
                {
                    GeneratorBuyView generatorBuyView = Instantiate(generatorViewPrefab, scrollViewHolder).GetComponent<GeneratorBuyView>();
                    generatorBuyView.Init(generatorStats[i], onTryBuyGenerator, onEnableTooltip, onDisableTooltip);
                    generatorBuyViews.Add(generatorBuyView);
                }
            }
        }

        public void UpdateGenerator(GeneratorData generatorData)
        {
            for (int i = 0; i < generatorBuyViews.Count; i++)
            {
                if (generatorBuyViews[i].Id == generatorData.numId)
                {
                    generatorBuyViews[i].UpdateData(generatorData);
                    return;
                }
            }
        }

        public void AddGenerator(GeneratorData generatorData)
        {
            if (generatorBuyViews.Find(gen => gen.Id == generatorData.numId) == null)
            {
                GeneratorBuyView generatorBuyView = Instantiate(generatorViewPrefab, scrollViewHolder).GetComponent<GeneratorBuyView>();
                generatorBuyView.Init(generatorData, onTryBuyGenerator, onEnableTooltip, onDisableTooltip);
                generatorBuyViews.Add(generatorBuyView);
            }
        }

        public void OnEnergyUpdate(long newEnergy)
        {
            for (int i = 0; i < generatorBuyViews.Count; i++)
            {
                generatorBuyViews[i].OnUpdateEnergy(newEnergy);
            }
        }
    }
}