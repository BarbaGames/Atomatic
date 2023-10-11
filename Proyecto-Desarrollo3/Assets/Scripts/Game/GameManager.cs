using System.Collections.Generic;
using Generators;
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
        [SerializeField] private GameplayView gameplayView = null;

        private const string EnergyKey = "energy";
        private long energy = 0;
        private List<Generator> generators = null;
        private const int MaxTime = 1;
        private float timer = MaxTime;

        private void Start()
        {
            if (FileHandler.FileExist(EnergyKey))
            {
                if (FileHandler.TryLoadFileRaw(EnergyKey, out string data))
                {
                    AddCurrency(long.Parse(data));
                }
            }

            gameplayView.Init(generatorSoData.generators, BuyGenerator, PlayerClick);

            generators = new List<Generator>();

            for (int i = 0; i < generatorSoData.generators.Count; i++)
            {
                Generator generator = Instantiate(generatorPrefab, transform).GetComponent<Generator>();
                generator.Init(Instantiate(generatorSoData.generators[i]));
                generators.Add(generator);
            }
        }

        private void OnEnable()
        {
            ButtonsController.onUpgradeUnlocked += UpgradeGenerator;
        }

        private void OnDisable()
        {
            ButtonsController.onUpgradeUnlocked -= UpgradeGenerator;
        }

        private void Update()
        {
            GeneratorsLoop();
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
            timer -= Time.deltaTime;

            if (timer > 0) return;

            timer = MaxTime;
            
            long generated = 0;
            
            for (int i = 1; i < generators.Count; i++)
            {
                if (!generators[i].IsActive) continue;

                generated += generators[i].Generate();
            }

            if(generated > 0) AddCurrency(generated);
        }

        private void AddCurrency(long energyToAdd)
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
        }

        private void BuyGenerator(string id)
        {
            Generator generator = generators.Find(i => i.GeneratorData.id == id);

            if (generator != null)
            {
                if (energy < generator.GeneratorData.levelUpCost) return;

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

                gameplayView.UpdateGenerator(generator.GeneratorData);

                UpdateEnergyPerSecond();
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