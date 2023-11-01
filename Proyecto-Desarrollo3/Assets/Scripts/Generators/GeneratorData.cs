using System;

using Newtonsoft.Json;

using UnityEngine;

namespace Generators
{
    [CreateAssetMenu(fileName = "GeneratorStats")]
    [Serializable]
    public class GeneratorData : ScriptableObject
    {
        [SerializeField] public string id;
        [JsonIgnore] public Sprite icon;
        [JsonIgnore] public Sprite background;
        [SerializeField] public float levelUpCostIncrease;
        [SerializeField] public long levelUpCost;
        [SerializeField] public long currencyGenerated;
        [SerializeField] public long baseCurrencyGenerated;
        [SerializeField] public string description;
        [SerializeField] public int maxTimer = 1;
        [SerializeField] public float timer;
        [SerializeField] public int level = 0;
        [SerializeField] public bool unlocked = false;
    }
}