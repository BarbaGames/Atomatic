using System.Collections.Generic;

using Generators;
using Newtonsoft.Json;
using Progress;
using UI;
using UnityEngine;


namespace Game
{
    /// <summary>
    /// Manages game elements such as the player, generators and upgrades.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject generatorPrefab = null;
        [SerializeField] private GeneratorSO generatorSoData = null;
        [SerializeField] private Upgrades upgradesData = null;
        [SerializeField] private GameplayView gameplayView = null;

        private const string EnergyKey = "energy";
        private const string GeneratorsKey = "generators";
        private const string UpgradesKey = "upgrades";

        private long energy = 0;
        private List<Generator> generators = null;
        private List<Upgrade> upgrades = null;

        private void Start()
        {
            upgrades = new List<Upgrade>();
            for (int i = 0; i < upgradesData.upgrades.Count; i++)
            {
                upgrades.Add(Instantiate(upgradesData.upgrades[i]));
            }

            gameplayView.Init(upgrades, BuyUpgrade, PlayerClick);
            generators = new List<Generator>();

            for (int i = 0; i < generatorSoData.generators.Count; i++)
            {
                Generator generator = Instantiate(generatorPrefab, transform).GetComponent<Generator>();
                generator.Init(Instantiate(generatorSoData.generators[i]));
                generators.Add(generator);
            }
            
            if (FileHandler.TryLoadFileRaw(GeneratorsKey, out string generatorsDataString))
            {
                List<GeneratorData> savedGeneratorsData =
                    JsonConvert.DeserializeObject<List<GeneratorData>>(generatorsDataString);
                
                gameplayView.InitGeneratorsBuyView(generatorSoData.generators, BuyGenerator, false);

                for (int i = 0; i < savedGeneratorsData.Count; i++)
                {
                    Generator generator = generators.Find(g => g.GeneratorData.id == savedGeneratorsData[i].id);
                    if (generator != null && savedGeneratorsData[i].unlocked)
                    {
                        generator.SetData(savedGeneratorsData[i]);
                        generator.GeneratorData.unlocked = true;
                        generator.IsActive = true;
                        generator.gameObject.SetActive(true);
                        gameplayView.AddGenerator(generator.GeneratorData);
                        gameplayView.UnlockGenerator(generator.GeneratorData);
                        gameplayView.UpdateGenerator(generator.GeneratorData);

                        if (i == savedGeneratorsData.Count - 1)
                        {
                            if (i + 1 < generators.Count && !generators[i + 1].IsActive)
                            {
                                gameplayView.AddGenerator(generators[i + 1].GeneratorData);
                            }
                        }
                    }
                }
            }
            else
            {
                gameplayView.InitGeneratorsBuyView(generatorSoData.generators, BuyGenerator, true);
            }
        

            if (FileHandler.TryLoadFileRaw(UpgradesKey, out string upgradesDataString))
            {
                List<Upgrade> savedUpgrades = JsonConvert.DeserializeObject<List<Upgrade>>(upgradesDataString);

                for (int i = 0; i < upgrades.Count; i++)
                {
                    Upgrade upgrade = savedUpgrades.Find(u => u.id == upgrades[i].id);

                    if (upgrade != null && upgrade.bought)
                    {
                        upgrades[i] = upgrade;
                        gameplayView.UpdateUpgrade(upgrades[i]);
                    }
                }
            }

            if (FileHandler.TryLoadFileRaw(EnergyKey, out string energyDataString))
            {
                AddCurrency(long.Parse(energyDataString));
            }
        }

        private void Update()
        {
            GeneratorsLoop();
        }

        private void OnApplicationQuit()
        {
            FileHandler.SaveFile(EnergyKey, energy.ToString());

            List<GeneratorData> generatorsData = new List<GeneratorData>();
            for (int i = 0; i < generators.Count; i++)
            {
                if (generators[i].GeneratorData.unlocked)
                {
                    generatorsData.Add(generators[i].GeneratorData);
                }
            }

            FileHandler.SaveFile(GeneratorsKey, JsonConvert.SerializeObject(generatorsData));
            FileHandler.SaveFile(UpgradesKey, JsonConvert.SerializeObject(upgrades));
        }

        public void UpgradeGenerator(long price, int id)
        {
            const int multiplier = 2;
            if (energy > price)
            {
                generators[id].GeneratorData.currencyGenerated *= multiplier;
                generators[id].GeneratorData.baseCurrencyGenerated *= multiplier;
                RemoveCurrency(price);
                UpdateEnergyPerSecond();
            }
        }

        private void GeneratorsLoop()
        {
            long generated = 0;

            for (int i = 1; i < generators.Count; i++)
            {
                if (!generators[i].IsActive) continue;

                generated += generators[i].Generate();
            }

            if (generated > 0) AddCurrency(generated);
        }

        /// <summary>
        /// Function for debugging
        /// </summary>
        public void DebugAddCurrency()
        {
            energy += 1000000;
            gameplayView.UpdateEnergy(energy);
        }
        
        /// <summary>
        /// Function for debugging
        /// </summary>
        public void DebugClearLocalData()
        {
            FileHandler.DeleteAllFiles();
        }

        public void AddCurrency(long energyToAdd)
        {
            energy += energyToAdd;
            gameplayView.UpdateEnergy(energy);
        }

        private void RemoveCurrency(long energyToRemove)
        {
            energy -= energyToRemove;
            gameplayView.UpdateEnergy(energy);
        }

        private void UpdateEnergyPerSecond()
        {
            long generationPerSec = 0;

            for (int i = 0; i < generators.Count; i++)
            {
                if (i == 0) continue;
                if (!generators[i].IsActive) continue;

                generationPerSec += generators[i].GeneratorData.currencyGenerated;
            }

            gameplayView.UpdateEnergyPerSec(generationPerSec);
        }

        private void PlayerClick()
        {
            long energyGenerated = generators[0].Generate();

            if (energyGenerated <= 0) return;

            AddCurrency(energyGenerated);

            gameplayView.SpawnFlyingText(energyGenerated);
            AkSoundEngine.PostEvent("ClickGenerator", gameObject); // Wwise evento de click
        }

        private void BuyGenerator(int id)
        {
            Generator generator = generators.Find(i => i.GeneratorData.numId == id);

            if (generator != null)
            {
                if (energy < generator.GeneratorData.levelUpCost)
                {
                    AkSoundEngine.PostEvent("BuyNegative", gameObject); // Wwise evento de BuyNegative
                    return;
                }

                if (!generator.IsActive)
                {
                    UnlockGenerator(generator);
                    gameplayView.UnlockGenerator(generator.GeneratorData);

                    if (id + 1 < generators.Count && !generators[id + 1].IsActive) 
                    {
                        gameplayView.AddGenerator(generators[id + 1].GeneratorData);
                    }
                }
                else
                {
                    RemoveCurrency(generator.GeneratorData.levelUpCost);
                    generator.Upgrade();
                }

                generator.PlayAudio(gameObject);
                AkSoundEngine.PostEvent("BuyShop", gameObject); // Wwise evento de BuyShop
                gameplayView.UpdateGenerator(generator.GeneratorData);

                UpdateEnergyPerSecond();
            }
        }

        private void BuyUpgrade(int id)
        {
            for (int i = 0; i < upgrades.Count; i++)
            {
                if (upgrades[i].id == id)
                {
                    if (energy < upgrades[i].price)
                    {
                        AkSoundEngine.PostEvent("BuyNegative", gameObject); // Wwise evento de BuyNegative
                        return;
                    }

                    RemoveCurrency(upgrades[i].price);
                    upgrades[i].bought = true;
                    gameplayView.UpdateUpgrade(upgrades[i]);
                    AkSoundEngine.PostEvent("BuyUpgrade", gameObject); // Wwise evento de BuyUpgrade
                    return;
                }
            }
        }

        private void UnlockGenerator(Generator generator)
        {
            RemoveCurrency(generator.GeneratorData.levelUpCost);
            generator.GeneratorData.unlocked = true;
            generator.Upgrade();
            generator.IsActive = true;
            generator.gameObject.SetActive(true);
        }
    }
}