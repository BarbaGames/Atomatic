using System.Globalization;
using Code.Scripts.Game;
using UnityEngine;

namespace Code.Scripts.Generators
{
    public class Generator : MonoBehaviour
    {
        [SerializeField] public GeneratorStats generatorStats;
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
                foreach (GameObject textHolder in GameManager.textHolderPool)
                {
                    if (!textHolder.activeSelf)
                    {
                        textHolder.SetActive(true);
                        textHolder.transform.position = transform.position;
                        textHolder.GetComponent<TextManager>()
                            .SetText("+" + generatorStats.currencyGenerated.ToString(CultureInfo.InvariantCulture));
                        break;
                    }
                }

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
                double cost = generatorStats.levelUpCost;
                generatorStats.levelUpCost *= generatorStats.levelUpCostIncrease;
                generatorStats.currencyGenerated = generatorStats.currencyGeneratedIncrease * _level;

                return cost;
            }

            return 0;
        }
    }
}