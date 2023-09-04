using Code.Scripts.Achievements;
using Code.Scripts.Game;
using UnityEngine;

namespace Code.Scripts.Player
{
    public class Wallet : MonoBehaviour
    {
        public double Currency { get; private set; }

        private Achievement[] _achievements; //scriptableObject?
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
            // float modifier = 0;
            // float multiplier = 0;
            //
            // foreach (Achievement achievement in _achievements)
            // {
            //     achievement.Modify(ref value);
            // }
            // Currency += value * multiplier + modifier;
            Currency += value;
            if (OnValueUpdated != null)
            {
                OnValueUpdated();
            }
            PlayerPrefs.SetInt("currency", (int)Currency);
            PlayerPrefs.Save();
        }
    }
}