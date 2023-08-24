using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Scripts.Generators
{
    public class Generator : MonoBehaviour
    {
        [SerializeField] public GeneratorStats generatorStats;
        [SerializeField] public GameObject textHolderPrefab;
        public bool IsActive { get; set; }
        private int _level = 1;
        private float _timer = 0;
        public float TimerMax { get; set; }
        public float LevelUpCostIncrease { get; set; }
        public double LevelUpCost { get; set; }
        public double CurrencyGenerated { get; set; }
        public double CurrencyGeneratedIncrease { get; set; }
        public string Description { get; set; }

        private void Start()
        {
            TimerMax = generatorStats.timerMax;
            LevelUpCostIncrease = generatorStats.levelUpCostIncrease;
            LevelUpCost = generatorStats.levelUpCost;
            CurrencyGenerated = generatorStats.currencyGenerated;
            CurrencyGeneratedIncrease = generatorStats.currencyGeneratedIncrease;
            Description = generatorStats.description;
        }

        /// <summary>
        /// After a certain time or condition, returns currency generated.
        /// </summary>
        /// <returns> currency generated </returns>
        public double Generate()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                _timer = TimerMax;
                GameObject textInstance = Instantiate(textHolderPrefab, transform);
                textInstance.GetComponent<TextManager>().SetText(CurrencyGenerated.ToString(CultureInfo.InvariantCulture));
                return CurrencyGenerated;
            }
            return 0;
        }

        /// <summary>
        /// Upgrade generator's currency generated
        /// </summary>
        /// <param name="currency"> Player's currency </param>
        public double Upgrade(double currency)
        {
            if (currency > LevelUpCost)
            {
                _level++;
                LevelUpCost *= LevelUpCostIncrease;
                CurrencyGenerated = CurrencyGeneratedIncrease * _level;
                
                return LevelUpCost;
            }

            return 0;
        }
    }
}