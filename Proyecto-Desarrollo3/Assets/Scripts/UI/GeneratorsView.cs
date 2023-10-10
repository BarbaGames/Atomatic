using System;
using System.Collections.Generic;

using BarbaGames.Game.Generators;

using UnityEngine;

public class GeneratorsView : MonoBehaviour
{
    [SerializeField] private GameObject generatorViewGo = null;
    [SerializeField] private Transform scrollViewHolder = null;

    private List<GeneratorView> generatorViews = null;

    public void Init(List<GeneratorData> generatorStats, Action<GeneratorData> onEnableTooltip, Action onDisableTooltip)
    {

    }
    
    public void UpdateGenerator()
    {
        
    }
}