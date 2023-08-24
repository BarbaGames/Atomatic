using Code.Scripts.Generators;
using Code.Scripts.Player;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Scripts.Game
{
    /// <summary>
    /// Manages game elements such as the player, generators and upgrades.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text currencyText;
        [SerializeField] private Generator[] generators;
        [SerializeField] private GeneratorStats playerGeneratorStats;
        [SerializeField] private GameObject textHolderPrefab;

        private const short GeneratorsQty = 5;

        private Generator _playerGenerator;
        private PlayerManager _playerManager;
        private PlayerView _playerView;

        public delegate void CurrencyEventHandler(double value);

        public static event CurrencyEventHandler OnCurrencyEvent;

        private void Awake()
        {
            InitPlayerComponents();
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
            foreach (Generator generator in generators)
            {
                if (!generator.IsActive) continue;
                double generated = generator.Generate();
                if (generated <= 0) continue;

                AddCurrency(generated);
            }
        }

        private void InitPlayerComponents()
        {
            _playerGenerator = gameObject.AddComponent<Generator>();
            _playerGenerator.generatorStats =  playerGeneratorStats;
            _playerGenerator.textHolderPrefab = textHolderPrefab;
            _playerManager = gameObject.AddComponent<PlayerManager>();
            _playerManager.Wallet = gameObject.AddComponent<Wallet>();
            _playerView = gameObject.AddComponent<PlayerView>();
            _playerView.CurrencyText = currencyText;
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
            RemoveCurrency(generators[id].Upgrade(_playerManager.Wallet.Currency));
        }

        public void UpgradePlayerGenerator()
        {
            RemoveCurrency(_playerGenerator.Upgrade(_playerManager.Wallet.Currency));
        }

        [ContextMenu("Unlock basic Generator")]
        public void UnlockGenerator()
        {
            generators[0].IsActive = true;
        }
        
        [ContextMenu("Upgrade basic Generator")]
        public void UpgradeBasicGenerator()
        {
            generators[0].Upgrade(_playerManager.Wallet.Currency);
        }
    }
}