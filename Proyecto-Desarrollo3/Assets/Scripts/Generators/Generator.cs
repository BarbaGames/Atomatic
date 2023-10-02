using UnityEngine;

namespace BarbaGames.Game.Generators
{
    public class Generator : MonoBehaviour
    {
        private GeneratorData generatorData;
        private int level = 1;
        private float timer = 0;

        public GeneratorData GeneratorData { get => generatorData; }
        public bool IsActive { get; set; }

        public void Init(GeneratorData generatorData) { this.generatorData = generatorData; }

        /// <summary>
        /// After a certain time or condition, returns currency generated.
        /// </summary>
        /// <returns> currency generated </returns>
        public long Generate()
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = generatorData.timerMax;
                // foreach (GameObject textHolder in GameManager.textHolderPool)
                // {
                //     if (!textHolder.activeSelf)
                //     {
                //         // textHolder.SetActive(true);
                //         // textHolder.transform.position = transform.position;
                //         // textHolder.GetComponent<TextManager>().SetText("+" + generatorData.currencyGenerated.ToString(CultureInfo.InvariantCulture));
                //         // break;
                //     }
                // }

                return generatorData.currencyGenerated;
            }

            return 0;
        }

        /// <summary>
        /// Upgrade generator's currency generated
        /// </summary>
        /// <param name="currency"> Player's currency </param>
        public void Upgrade()
        {
            level++;
            long cost = generatorData.levelUpCost;
            generatorData.levelUpCost =  (long)(generatorData.levelUpCost * generatorData.levelUpCostIncrease);
            generatorData.currencyGenerated = generatorData.currencyGeneratedIncrease * level;
            
        }
    }
}