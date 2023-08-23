using TMPro;
using UnityEngine;
using System.Globalization;

namespace Code.Scripts.Player
{
    public class PlayerView : MonoBehaviour
    {
        public TMP_Text CurrencyText { get; set; }

        private void OnEnable()
        {
            Wallet.OnValueUpdated += UpdateCurrency;
        }

        private void OnDisable()
        {
            Wallet.OnValueUpdated -= UpdateCurrency;
        }
        
        //Sound
        //Achievements
        //Show currency
        
        private void UpdateCurrency(double value)
        {
            CurrencyText.text = value.ToString(CultureInfo.CurrentCulture);
        }
    }
}