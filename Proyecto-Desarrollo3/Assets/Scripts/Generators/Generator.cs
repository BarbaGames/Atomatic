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

        public void SetData(GeneratorData generatorData)
        {
            this.generatorData.level = generatorData.level;
            this.generatorData.levelUpCost = generatorData.levelUpCost;
            this.generatorData.currencyGenerated = generatorData.currencyGenerated;
        }
        
        /// <summary>
        /// After a certain time or condition, returns currency generated.
        /// </summary>
        /// <returns> currency generated </returns>
        public long Generate()
        {
            generatorData.timer -= Time.deltaTime;
            
            if (generatorData.timer > 0) return 0;
            
            generatorData.timer = generatorData.maxTimer;
            
            return generatorData.currencyGenerated;
        }

        /// <summary>
        /// Upgrade generator's currency generated
        /// </summary>
        /// <param name="currency"> Player's currency </param>
        public void Upgrade()
        {
            generatorData.level++;
            generatorData.levelUpCost = (long)(generatorData.levelUpCost * generatorData.levelUpCostIncrease);
            generatorData.currencyGenerated = generatorData.baseCurrencyGenerated * generatorData.level;
        }
    }
}