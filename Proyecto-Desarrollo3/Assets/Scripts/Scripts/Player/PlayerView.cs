using TMPro;
using UnityEngine;
using System.Globalization;
using Code.Scripts.Game;
using UnityEngine.Serialization;

namespace Code.Scripts.Player
{
    public class PlayerView : MonoBehaviour
    {
        public TMP_Text CurrencyText { get; set; }
        public TMP_Text[] generatorsUiText;
        public Wallet wallet;
        private void OnEnable()
        {
            Wallet.OnValueUpdated += UpdateCurrency;
            GameManager.OnGeneratorChangeEvent += UpdateGeneratorsUiText;
        }

        private void OnDisable()
        {
            Wallet.OnValueUpdated -= UpdateCurrency;
            GameManager.OnGeneratorChangeEvent -= UpdateGeneratorsUiText;
        }
        
        //TODO
        //Sounds
        //Achievements
        //Show currency
        //Show unlock/upgrade generator costs
        //Show buy upgrades

        private void UpdateGeneratorsUiText(short id, int cost)
        {
            generatorsUiText[id].text = cost.ToString(CultureInfo.CurrentCulture);
        }
        
        private void UpdateCurrency()
        {
            int currency = (int)wallet.Currency;
            CurrencyText.text = currency.ToString(CultureInfo.CurrentCulture);
        }
    }
}