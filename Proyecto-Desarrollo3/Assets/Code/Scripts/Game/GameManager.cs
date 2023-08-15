using TMPro;
using UnityEngine;

namespace Code.Scripts.Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text currency;
        public int userLvl = 1;
        private int intValue = 10;
        public delegate void DoubleValueEventHandler(double value);
        public static event DoubleValueEventHandler OnValueReceived;

        
        private void OnEnable()
        {
            Player.OnValueUpdated += UpdateCurrency;
        }

        private void OnDisable()
        {
            Player.OnValueUpdated -= UpdateCurrency;
        }
        
        public void SendDoubleValue(double valueToSend)
        {
                OnValueReceived?.Invoke(valueToSend);
        }
        public void SendFloatValue(float valueToSend)
        {
            OnValueReceived?.Invoke(valueToSend);
        }
        public void UpdateCurrency(double value)
        {
            currency.text = value.ToString();
        }

        public void UpgradeLevel()
        {
            userLvl++;
        }

        [ContextMenu("Send 10 coins")]
        private void Send10Coins()
        {
            OnValueReceived?.Invoke(intValue);
        }
    }
}
