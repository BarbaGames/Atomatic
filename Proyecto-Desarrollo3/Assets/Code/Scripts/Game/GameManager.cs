using Code.Scripts.Generators;
using Code.Scripts.Player;
using TMPro;
using UnityEngine;

namespace Code.Scripts.Game
{
    /// <summary>
    /// Manages game elements such as the player, generators and upgrades.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text currencyText;

        private const short GeneratorsQty = 5;

        private Generator[] _generators;
        private Generator _playerGenerator;
        private PlayerManager _playerManager;
        private PlayerView _playerView;
        public delegate void CurrencyEventHandler(double value);
        public static event CurrencyEventHandler OnCurrencyEvent;

        private void Awake()
        {
            InitPlayerGenerator();
            
            _generators = new Generator[GeneratorsQty];
            _playerManager = gameObject.AddComponent<PlayerManager>();
            _playerManager.Wallet = gameObject.AddComponent<Wallet>();
            _playerView = gameObject.AddComponent<PlayerView>();
            _playerView.CurrencyText = currencyText;
        }

        private void OnEnable()
        {
            InputManager.OnClickEvent += CheckClick;
        }

        private void OnDisable()
        {
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

        private void RemoveCurrency(double currency)
        {
            OnCurrencyEvent?.Invoke(-currency);
        }

        private void CheckClick()
        {
            Ray ray = Camera.main!.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit))
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

        public void PlayerClick()
        {
            AddCurrency(_playerGenerator.Generate());
        }
        public void UpgradeLevel(int id)
        {
            RemoveCurrency(_generators[id].Upgrade(_playerManager.Wallet.Currency));
        }

        public void UpgradePlayerGenerator()
        {
             RemoveCurrency(_playerGenerator.Upgrade(_playerManager.Wallet.Currency));
        }
    }
}