using System.Collections.Generic;

using Generators;
using Newtonsoft.Json;
using Progress;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        [SerializeField] private GameObject[] stages = null;

        private const string EnergyKey = "energy";
        private const string GeneratorsKey = "generators";
        private const string UpgradesKey = "upgrades";

        private long energy = 0;
        private List<Generator> generators = null;
        private List<Upgrade> upgrades = null;

        private bool deleteSavedData = false;

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
                    Generator generator = generators.Find(g => g.GeneratorData.numId == savedGeneratorsData[i].numId);
                    if (generator != null && savedGeneratorsData[i].unlocked)
                    {
                        generator.SetData(savedGeneratorsData[i]);
                        generator.GeneratorData.unlocked = true;
                        generator.IsActive = true;
                        generator.gameObject.SetActive(true);
                        gameplayView.AddGenerator(generator.GeneratorData);
                        gameplayView.UnlockGenerator(generator.GeneratorData);
                        gameplayView.UpdateGenerator(generator.GeneratorData, true);

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
                        gameplayView.UpdateUpgrade(upgrades[i], null);
                    }
                }
            }

            if (FileHandler.TryLoadFileRaw(EnergyKey, out string energyDataString))
            {
                AddCurrency(long.Parse(energyDataString));
            }
            AddCurrency(99999999);
            UpdateEnergyPerSecond();
        }

        private void Update()
        {
            GeneratorsLoop();
        }

        private void OnApplicationQuit()
        {
            if (deleteSavedData)
            {
                FileHandler.DeleteAllFiles();
                return;
            }
            
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
            deleteSavedData = true;
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
                    gameplayView.UnlockUpgrade(generator.GeneratorData);

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
                gameplayView.UpdateGenerator(generator.GeneratorData, false);

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

                    gameplayView.UpdateUpgrade(upgrades[i], null);

                    AkSoundEngine.PostEvent("BuyUpgrade", gameObject); // Wwise evento de BuyUpgrade
                    if (upgrades[i].stageUpgrade)
                    {
                        if (upgrades[i].stageUnlock > stages.Length)
                        {
                            SceneManager.LoadScene("Win");
                        }
                        else
                        {
                            stages[upgrades[i].stageUnlock - 2].SetActive(false);
                            stages[upgrades[i].stageUnlock - 1].SetActive(true);
                            
                            string generatorState = "";
                            switch (upgrades[i].stageUnlock)
                            {
                                case 1: generatorState = "One"; break;
                                case 2: generatorState = "Two"; break;
                                case 3: generatorState = "Three"; break;
                            }
                            AkSoundEngine.SetState("CurrentGenerator", generatorState);
                        }
                    }
                    else
                    {
                        gameplayView.UnlockUpgrade(upgrades[i]);
                    }
                    
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