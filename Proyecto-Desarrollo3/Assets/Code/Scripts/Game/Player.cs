using UnityEngine;

namespace Code.Scripts.Game
{
    public class Player : MonoBehaviour
    {
        private double _currency = 0.0;
        public static int UserLvl { get; set; }

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
            _currency += value;
            if (OnValueUpdated != null)
            {
                OnValueUpdated(_currency);
            }
        }
    }
}