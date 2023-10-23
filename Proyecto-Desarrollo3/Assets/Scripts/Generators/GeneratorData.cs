using UnityEngine;

namespace Generators
{
    [CreateAssetMenu(fileName = "GeneratorStats")]
    public class GeneratorData : ScriptableObject
    {
        [SerializeField] public string id;
        [SerializeField] public Sprite icon;
        [SerializeField] public float levelUpCostIncrease;
        [SerializeField] public long levelUpCost;
        [SerializeField] public long currencyGenerated;
        [SerializeField] public long baseCurrencyGenerated;
        [SerializeField] public int MaxTimer = 1;
        public int level = 0;
        public float timer = 1;
        public bool unlocked = false;
    }
}