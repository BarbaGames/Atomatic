using UnityEngine;

namespace BarbaGames.Game.Generators
{
    [CreateAssetMenu(fileName = "GeneratorStats")]
    public class GeneratorData : ScriptableObject
    {
        [SerializeField] public string id;
        [SerializeField] public float timerMax;
        [SerializeField] public float levelUpCostIncrease;
        [SerializeField] public long levelUpCost;
        [SerializeField] public long currencyGenerated;
        [SerializeField] public long currencyGeneratedIncrease;
        [SerializeField] public string description;
        public int level = 1;
    }
}