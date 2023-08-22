using System;
using System.Globalization;
using Code.Scripts.Generators;
using TMPro;
using UnityEngine;

namespace Code.Scripts.Game
{
    /// <summary>
    /// Manages game elemets such as the player, generators and upgrades.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text currencyText;

        private const short GeneratorsQty = 5;

        private Generator[] _generators;
        private Generator _playerGenerator;
        private Player _player;

        public delegate void CurrencyEventHandler(double value);

        public static event CurrencyEventHandler OnCurrencyEvent;

        private void Awake()
        {
            InitPlayerGenerator();
            _generators = new Generator[GeneratorsQty];
            _player = gameObject.AddComponent<Player>();
        }

        private void OnEnable()
        {
            Player.OnValueUpdated += UpdateCurrency;
            InputManager.OnClickEvent += CheckClick;
        }

        private void OnDisable()
        {
            Player.OnValueUpdated -= UpdateCurrency;
            InputManager.OnClickEvent -= CheckClick;
        }

        private void Update()
        {
            foreach (Generator generator in _generators)
            {
                if(generator == null) continue;
                if (!generator.IsActive) continue;
                double generated = generator.Generate();
                if (generated <= 0) continue;
            
                //TODO better way to manage this event?
                AddCurrency(generated);
            }
        }

        private void InitPlayerGenerator()
        {
            _playerGenerator = new Generator
            {
                IsActive = true,
                TimerMax = 0,
                CurrencyGenerated = 1,
                LevelUpCost = 1,
                CurrencyGeneratedIncrease = 1.15
            };
        }
        private void AddCurrency(double currency)
        {
            OnCurrencyEvent?.Invoke(currency);
        }

        private void CheckClick()
        {
            Ray ray = Camera.main!.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject)
                {
                    if (hit.transform.GetComponent<Clickable>())
                    {
                        AddCurrency(_playerGenerator.Generate());
                    }
                }
            }
        }

        private void UpdateCurrency(double value)
        {
            currencyText.text = value.ToString(CultureInfo.CurrentCulture);
        }

        public void UpgradeLevel(int id)
        {
            _generators[id].Upgrade(_player.Currency);
        }

        public void UpgradePlayerGenerator()
        {
            _playerGenerator.Upgrade(_player.Currency);
        }
    }
}