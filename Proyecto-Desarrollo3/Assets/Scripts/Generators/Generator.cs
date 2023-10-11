using UnityEngine;

namespace Generators
{
    public class Generator : MonoBehaviour
    {
        private GeneratorData generatorData;

        public GeneratorData GeneratorData
        {
            get => generatorData;
        }

        public bool IsActive { get; set; }

        public void Init(GeneratorData generatorData)
        {
            this.generatorData = generatorData;
        }

        /// <summary>
        /// After a certain time or condition, returns currency generated.
        /// </summary>
        /// <returns> currency generated </returns>
        public long Generate()
        {
            return generatorData.currencyGenerated;
        }

        /// <summary>
        /// Upgrade generator's currency generated
        /// </summary>
        /// <param name="currency"> Player's currency </param>
        public void Upgrade()
        {
            generatorData.level++;
            long cost = generatorData.levelUpCost;
            generatorData.levelUpCost = (long)(generatorData.levelUpCost * generatorData.levelUpCostIncrease);
            generatorData.currencyGenerated = generatorData.baseCurrencyGenerated * generatorData.level;
        }
    }
}