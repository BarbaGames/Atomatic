using UnityEngine;

namespace Code.Scripts.Game
{
    public class Player : MonoBehaviour
    {
        private double currency = 0.0;
        
        public delegate void CurrencyUpdatedEventHandler(double value);
        public static event CurrencyUpdatedEventHandler OnValueUpdated;
        private void OnEnable()
        {
            GameManager.OnValueReceived += HandleValue;
        }

        private void OnDisable()
        {
            GameManager.OnValueReceived -= HandleValue;
        }

        private void HandleValue(double value)
        {
            currency += value;
            if (currency != null && OnValueUpdated != null)
            {
                OnValueUpdated(currency);
            }
        }
    
    
    
    }
}
