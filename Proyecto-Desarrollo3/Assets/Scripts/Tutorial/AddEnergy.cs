using Game;
using Generators;
using UnityEngine;

namespace Tutorial
{
    public class AddEnergy : MonoBehaviour
    {
        [SerializeField] private GeneratorData firstGenerator;
        [SerializeField] private GameManager gameManager;
        private bool _added = false;

        private void OnEnable()
        {
            if(_added) return;
            gameManager.AddCurrency(firstGenerator.levelUpCost);
            _added = true;
        }
    }
}