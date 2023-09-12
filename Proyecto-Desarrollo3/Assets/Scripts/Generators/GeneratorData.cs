using UnityEngine;

namespace BarbaGames.Game.Generators
{
    [CreateAssetMenu(fileName = "GeneratorStats")]
    public class GeneratorData : ScriptableObject
    {
        [SerializeField] public string id;
        [SerializeField] public float timerMax;
        [SerializeField] public float levelUpCostIncrease;
        [SerializeField] public double levelUpCost;
        [SerializeField] public double currencyGenerated;
        [SerializeField] public double currencyGeneratedIncrease;
        [SerializeField] public string description;
    }
}