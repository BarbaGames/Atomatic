using System;
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

            gameplayView.Init(generatorSoData.generators, upgrades, BuyGenerator, BuyUpgrade, PlayerClick);

            generators = new List<Generator>();

            for (int i = 0; i < generatorSoData.generators.Count; i++)
            {
                Generator generator = Instantiate(generatorPrefab, transform).GetComponent<Generator>();
                generator.Init(Instantiate(generatorSoData.generators[i]));
                generators.Add(generator);
            }

            if (FileHandler.FileExist(EnergyKey))
            {
                if (FileHandler.TryLoadFileRaw(EnergyKey, out string energyDataString))
                {
                    AddCurrency(long.Parse(energyDataString));
                }

                if (FileHandler.TryLoadFileRaw(GeneratorsKey, out string generatorsDataString))
                {
                    List<GeneratorData> generatorsData = JsonConvert.DeserializeObject<List<GeneratorData>>(generatorsDataString);

                    for (int i = 0; i < generatorsData.Count; i++)
                    {
                        Generator generator = generators.Find(g => g.GeneratorData.id == generatorsData[i].id);
                        if (generator != null && generatorsData[i].unlocked)
                        {
                            generator.SetData(generatorsData[i]);
                            generator.GeneratorData.unlocked = true;
                            generator.IsActive = true;
                            generator.gameObject.SetActive(true);
                            gameplayView.UnlockGenerator(generator.GeneratorData);
                            gameplayView.UpdateGenerator(generator.GeneratorData);
                        }
                    }
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
            }
        }
        private void Update() { GeneratorsLoop(); }

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
        /// The purpose is to use this function in a button for debugging purposes.
        /// </summary>
        public void DebugAddCurrency()
        {
            energy += 1000000;
            gameplayView.UpdateEnergy(energy);
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

        private void BuyGenerator(string id)
        {
            Generator generator = generators.Find(i => i.GeneratorData.id == id);

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
                }
                else
                {
                    RemoveCurrency(generator.GeneratorData.levelUpCost);
                    generator.Upgrade();
                }

                GeneratorSwitchSFXChange(id);

                //AkSoundEngine.SetSwitch("IncrementalBuyShopSwitches", "IncrementalBuyShop2", gameObject);
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
        private void GeneratorSwitchSFXChange(string id)
        {
            switch (id)
            {
                case "Basic":
                    AkSoundEngine.SetSwitch("IncrementalBuyShopSwitches", "IncrementalBuyShop1", gameObject);
                    break;
                case "Generator":
                    AkSoundEngine.SetSwitch("IncrementalBuyShopSwitches", "IncrementalBuyShop2", gameObject);
                    break;
                case "Small Energy Plant":
                    AkSoundEngine.SetSwitch("IncrementalBuyShopSwitches", "IncrementalBuyShop3", gameObject);
                    break;
                case "Solar Panels":
                    AkSoundEngine.SetSwitch("IncrementalBuyShopSwitches", "IncrementalBuyShop4", gameObject);
                    break;
                case "Coal Plants":
                    AkSoundEngine.SetSwitch("IncrementalBuyShopSwitches", "IncrementalBuyShop5", gameObject);
                    break;
                case "Nuclear Plant":
                    AkSoundEngine.SetSwitch("IncrementalBuyShopSwitches", "IncrementalBuyShop6", gameObject);
                    break;
                case "Fusion plant":
                    AkSoundEngine.SetSwitch("IncrementalBuyShopSwitches", "IncrementalBuyShop7", gameObject);
                    break;
                case "Geothermic plant":
                    AkSoundEngine.SetSwitch("IncrementalBuyShopSwitches", "IncrementalBuyShop8", gameObject);
                    break;
                case "Dyson sphere":
                    AkSoundEngine.SetSwitch("IncrementalBuyShopSwitches", "IncrementalBuyShop9", gameObject);
                    break;
                case "Star Harvester":
                    AkSoundEngine.SetSwitch("IncrementalBuyShopSwitches", "IncrementalBuyShop10", gameObject);
                    break;
                case "Alien Energy":
                    AkSoundEngine.SetSwitch("IncrementalBuyShopSwitches", "IncrementalBuyShop11", gameObject);
                    break;
                case "Black hole plant":
                    AkSoundEngine.SetSwitch("IncrementalBuyShopSwitches", "IncrementalBuyShop12", gameObject);
                    break;





            }


        }
    }
}


