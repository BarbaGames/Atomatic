using UnityEngine;

namespace Generators
{
    [CreateAssetMenu(fileName = "GeneratorStats")]
    public class GeneratorData : ScriptableObject
    {
        [SerializeField] public string id;
        [SerializeField] public Sprite icon;
        [SerializeField] public Sprite background;
        [SerializeField] public float levelUpCostIncrease;
        [SerializeField] public long levelUpCost;
        [SerializeField] public long currencyGenerated;
        [SerializeField] public long baseCurrencyGenerated;
        [SerializeField] public string description;
        [SerializeField] public int maxTimer = 1;
        public float timer;
        public int level = 0;
        public bool unlocked = false;
    }
}