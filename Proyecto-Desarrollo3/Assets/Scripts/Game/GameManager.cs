using System.Collections.Generic;

using Generators;
using Progress;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        [SerializeField] private LeaderboardHandler leaderboardHandler = null;

        [SerializeField] private GameObject pauseMenu = null;
        [SerializeField] private GameObject settingsMenu = null;
        [SerializeField] private Button debugIncreaseCurrency = null;
        [SerializeField] private Button debugClearData = null;
        
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
            
            gameplayView.InitGeneratorsBuyView(generatorSoData.generators, BuyGenerator, true);
            
            UpdateEnergyPerSecond();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                debugIncreaseCurrency.gameObject.SetActive(!debugIncreaseCurrency.gameObject.activeSelf);
                debugClearData.gameObject.SetActive(!debugClearData.gameObject.activeSelf);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseMenu.SetActive(!pauseMenu.activeSelf);
                if(settingsMenu.activeSelf) settingsMenu.SetActive(false);
            }
            GeneratorsLoop();
        }

        private void OnApplicationQuit()
        {
            if (deleteSavedData)
            {
                FileHandler.DeleteAllFiles();
                return;
            }
            
            leaderboardHandler.TryUpdateLeaderboard(energy);
        }
        
        private void GeneratorsLoop()
        {
            long generated = 0;

            for (int i = 1; i < generators.Count; i++)
            {
                if (!generators[i].IsActive) continue;

                generated += generators[i].Generate();
                if (generated > 0)
                {
                    List<Upgrade> genUpgrades = upgrades.FindAll(u => u.generatorId == i);
                    for (int j = 0; j < genUpgrades.Count; j++)
                    {
                        if (genUpgrades[j].bought)
                        {
                            generated += genUpgrades[j].currencyGeneratedAmount;
                        }
                    }
                    gameplayView.UpdateToolTip(generators[i].GeneratorData);
                }
            }

            if (generated > 0)
            {
                AddCurrency(generated);
            }
        }

        /// <summary>
        /// Function for debugging
        /// </summary>
        public void DebugAddCurrency()
        {
            energy *= 5;
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
                
                List<Upgrade> genUpgrades = upgrades.FindAll(u => u.generatorId == i);
                for (int j = 0; j < genUpgrades.Count; j++)
                {
                    if (genUpgrades[j].bought)
                    {
                        generationPerSec += genUpgrades[j].currencyGeneratedAmount;
                    }
                }
            }

            gameplayView.UpdateEnergyPerSec(generationPerSec);
        }
        
        private void PlayerClick()
        {
            long energyGenerated = generators[0].Generate();

            if (energyGenerated <= 0) return;
            
            List<Upgrade> genUpgrades = upgrades.FindAll(u => u.generatorId == 0);
            for (int j = 0; j < genUpgrades.Count; j++)
            {
                if (genUpgrades[j].bought)
                {
                    energyGenerated += genUpgrades[j].currencyGeneratedAmount;
                }
            }

            AddCurrency(energyGenerated);

            gameplayView.SpawnFlyingText(energyGenerated);
            AkSoundEngine.PostEvent("ClickGenerator", gameObject); // Wwise evento de click
        }

        public void BuyGenerator(int id)
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
                    upgrades[i].unlocked = true;

                    gameplayView.UpdateUpgrade(upgrades[i]);

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

                            string generatorState = upgrades[i].stageUnlock switch
                            {
                                1 => "One",
                                2 => "Two",
                                3 => "Three",
                                _ => "One"
                            };
                            AkSoundEngine.SetState("CurrentGenerator", generatorState);
                        }
                    }
                    else
                    {
                        if (gameplayView.UnlockUpgrade(upgrades[i]))
                        {
                            upgrades[i + 1].unlocked = true;
                        }
                    }
                    UpdateEnergyPerSecond();

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