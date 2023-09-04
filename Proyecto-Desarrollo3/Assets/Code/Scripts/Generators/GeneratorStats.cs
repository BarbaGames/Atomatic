using UnityEngine;

namespace Code.Scripts.Generators
{
    [CreateAssetMenu(fileName = "GeneratorStats")]
    public class GeneratorStats : ScriptableObject
    {
        [SerializeField] public float timerMax;
        [SerializeField] public float levelUpCostIncrease;
        [SerializeField] public double levelUpCost;
        [SerializeField] public double currencyGenerated;
        [SerializeField] public double currencyGeneratedIncrease;
        [SerializeField] public string description;
    }
}