using System;
using System.Collections.Generic;
using Generators;
using UnityEngine;

namespace UI
{
    public class GeneratorsView : MonoBehaviour
    {
        [SerializeField] private GameObject generatorViewGo = null;
        [SerializeField] private Transform scrollViewHolder = null;

        private List<GeneratorView> generatorViews = null;

        public void Init()
        {
            generatorViews = new List<GeneratorView>();
        }

        public void AddGenerator(GeneratorData generatorStats, Action<GeneratorData> onEnableTooltip, Action onDisableTooltip)
        {
            GeneratorView generatorView = Instantiate(generatorViewGo, scrollViewHolder).GetComponent<GeneratorView>();
            generatorView.Init(generatorStats, onEnableTooltip, onDisableTooltip);
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