using System.Collections.Generic;
using UnityEngine;
using BarbaGames.Game.Generators;
using BarbaGames.Game.UI;

namespace BarbaGames.Game
{
    /// <summary>
    /// Manages game elements such as the player, generators and upgrades.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject generatorPrefab = null;
        [SerializeField] private Generators.Generators generatorsDataSO = null;
        [SerializeField] private GameplayView gameplayView = null;

        private const string energyKey = "energy";
        private long energy = 0;
        private Generator playerClick = null;
        private List<Generator> generators = null;

        private void Start()
        {
            if (FileHandler.FileExist(energyKey))
            {
                if (FileHandler.TryLoadFileRaw(energyKey, out string data))
                {
                    AddCurrency(long.Parse(data));
                }
            }

            gameplayView.Init(generatorsDataSO.generators, UpgradeGenerator, PlayerClick);

            generators = new List<Generator>();
            for (int i = 0; i < generatorsDataSO.generators.Count; i++)
            {
                Generator generator = Instantiate(generatorPrefab, transform).GetComponent<Generator>();
                generator.Init(Instantiate(generatorsDataSO.generators[i]));
                generators.Add(generator);
            }
        }

        private void Update()
        {
            for (int i = 1; i < generators.Count; i++)
            {
                if (!generators[i].IsActive) continue;

                long generated = generators[i].Generate();
                if (generated <= 0) continue;
                AddCurrency(generated);
            }
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

        private void PlayerClick()
        {
            long energyGenerated = generators[0].Generate();
            if (energyGenerated <= 0) return;
            AddCurrency(energyGenerated);
            gameplayView.SpawnFlyingText(energyGenerated);
        }

        private void UpgradeGenerator(string id)
        {
            Generator generator = generators.Find(i => i.GeneratorData.id == id);

            if (generator != null)
            {
                if (energy < generator.GeneratorData.levelUpCost) return;

                if (!generator.IsActive)
                {
                    UnlockGenerator(generator);
                }
                else
                {
                    RemoveCurrency(generator.GeneratorData.levelUpCost);
                    generator.Upgrade();
                }

                gameplayView.UpdateGenerator(generator.GeneratorData);
            }
        }

        private void UnlockGenerator(Generator generator)
        {
            RemoveCurrency(generator.GeneratorData.levelUpCost);
            generator.Upgrade();
            generator.IsActive = true;
            generator.gameObject.SetActive(true);
        }
    }
}