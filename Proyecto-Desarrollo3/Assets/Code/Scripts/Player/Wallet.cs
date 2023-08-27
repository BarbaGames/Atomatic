using Code.Scripts.Game;
using UnityEngine;

namespace Code.Scripts.Player
{
    public class Wallet : MonoBehaviour
    {
        public double Currency { get; private set; }
        
        //TODO
        //Achievements
        //Decorator - procesador
        //currency multiplier

        public delegate void CurrencyUpdatedEventHandler();

        public static event CurrencyUpdatedEventHandler OnValueUpdated;

        private void OnEnable()
        {
            GameManager.OnCurrencyEvent += ModifyCurrency;
        }

        private void OnDisable()
        {
            GameManager.OnCurrencyEvent -= ModifyCurrency;
        }

        //TODO Decorator
        private void ModifyCurrency(double value)
        {
            Currency += value;
            if (OnValueUpdated != null)
            {
                OnValueUpdated();
            }
        }
    }
}