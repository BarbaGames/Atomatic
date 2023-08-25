using System.Globalization;
using UnityEngine;

namespace Code.Scripts.Generators
{
    public class Generator : MonoBehaviour
    {
        [SerializeField] public GeneratorStats generatorStats;
        [SerializeField] public GameObject textHolderPrefab;
        public bool IsActive { get; set; }
        private int _level = 1;
        private float _timer = 0;

        /// <summary>
        /// After a certain time or condition, returns currency generated.
        /// </summary>
        /// <returns> currency generated </returns>
        public double Generate()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                _timer = generatorStats.timerMax;
                GameObject textInstance = Instantiate(textHolderPrefab, transform);
                textInstance.GetComponent<TextManager>().SetText(generatorStats.currencyGenerated.ToString(CultureInfo.InvariantCulture));
                return generatorStats.currencyGenerated;
            }
            return 0;
        }

        /// <summary>
        /// Upgrade generator's currency generated
        /// </summary>
        /// <param name="currency"> Player's currency </param>
        public double Upgrade(double currency)
        {
            if (currency > generatorStats.levelUpCost)
            {
                _level++;
                generatorStats.levelUpCost *= generatorStats.levelUpCostIncrease;
                generatorStats.currencyGenerated =generatorStats.currencyGeneratedIncrease * _level;
                
                return generatorStats.levelUpCost;
            }

            return 0;
        }
    }
}