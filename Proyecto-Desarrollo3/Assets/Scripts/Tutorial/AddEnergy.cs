using Game;
using Generators;
using UnityEngine;

namespace Tutorial
{
    public class AddEnergy : MonoBehaviour
    {
        [SerializeField] private GeneratorData firstGenerator;
        [SerializeField] private GameManager gameManager;
        private long energy = 5;
        private bool added = false;
        private void Start()
        {
            firstGenerator.level = 0;
            energy = firstGenerator.levelUpCost;
        }

        private void OnEnable()
        {
            if(added) return;
            gameManager.AddCurrency(energy);
            added = true;
        }
    }
}