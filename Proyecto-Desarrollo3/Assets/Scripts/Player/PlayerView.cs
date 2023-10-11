using System.Globalization;
using TMPro;
using UnityEngine;

namespace Player
{
    public class PlayerView : MonoBehaviour
    {
        public TMP_Text CurrencyText { get; set; }
        public TMP_Text[] generatorsUiText;
        public Wallet wallet;
        private void OnEnable()
        {
            Wallet.OnValueUpdated += UpdateCurrency;
        }

        private void OnDisable()
        {
            Wallet.OnValueUpdated -= UpdateCurrency;
        }
        
        //TODO
        //Sounds
        //Achievements
        //Show currency
        //Show unlock/upgrade generator costs
        //Show buy upgrades

        private void UpdateGeneratorsUiText(short id, long cost)
        {
            generatorsUiText[id].text = cost.ToString(CultureInfo.CurrentCulture);
        }
        
        private void UpdateCurrency()
        {
            long currency = wallet.Currency;
            CurrencyText.text = currency.ToString(CultureInfo.CurrentCulture);
        }
    }
}