using System.Globalization;
using Code.Scripts.Generators;
using Code.Scripts.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField] private Button[] generatorButtons;

        private PlayerManager _playerManager;
        private PlayerView _playerView;

        public delegate void CurrencyEventHandler(double value);

        public static event CurrencyEventHandler OnCurrencyEvent;

        public delegate void GeneratorUpgradeEventHandler(short id, int cost);

        public static event GeneratorUpgradeEventHandler OnGeneratorChangeEvent;

        private void Awake()
        {
            InitPlayerComponents();
            if (PlayerPrefs.HasKey("currency"))
            {
                AddCurrency(PlayerPrefs.GetInt("currency"));
            }
        }

        private void Update()
        {
            for (int i = 0; i < generators.Length; i++)
            {
                if(i == 0) continue;
                if (!generators[i].IsActive) continue;
                double generated = generators[i].Generate();
                if (generated <= 0) continue;

                AddCurrency(generated);
            }

            PlayerPrefs.SetInt("currency", (int)_playerManager.Wallet.Currency);
            PlayerPrefs.Save();
        }

        private void InitPlayerComponents()
        {
            _playerManager = gameObject.AddComponent<PlayerManager>();
            _playerManager.Wallet = gameObject.AddComponent<Wallet>();

            _playerView = gameObject.AddComponent<PlayerView>();
            _playerView.CurrencyText = currencyText;
            _playerView.wallet = _playerManager.Wallet;

            _playerView.generatorsUiText = new TMP_Text[generatorButtons.Length];

            for (int i = 0; i < generatorButtons.Length; i++)
            {
                _playerView.generatorsUiText[i] = generatorButtons[i].GetComponentsInChildren<TMP_Text>()[0];
            }

            for (int i = 0; i < generators.Length; i++)
            {
                OnGeneratorChangeEvent?.Invoke((short)i, (int)generators[i].generatorStats.levelUpCost);
            }
        }

        private void AddCurrency(double currency)
        {
            OnCurrencyEvent?.Invoke(currency);
        }

        private void RemoveCurrency(double currency)
        {
            OnCurrencyEvent?.Invoke(-currency);
        }

        public void PlayerClick()
        {
            double currencyGenerated = generators[0].Generate();
            if (currencyGenerated <= 0) return;
            AddCurrency(currencyGenerated);

            Transform textTransform = transform;
            textTransform.position = Camera.main!.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward * 3;

            GameObject textInstance = Instantiate(textHolderPrefab, textTransform);

            textInstance.GetComponent<TextManager>().SetText(currencyGenerated.ToString(CultureInfo.InvariantCulture));
        }

        public void UpgradeLevel(int id)
        {
            if (!generators[id]) return;
            if(_playerManager.Wallet.Currency < generators[id].generatorStats.levelUpCost) return;
            
            if (!generators[id].IsActive)
            {
                UnlockGenerator(id);
            }
            else
            {
                RemoveCurrency(generators[id].Upgrade(_playerManager.Wallet.Currency));
                OnGeneratorChangeEvent?.Invoke((short)id, (int)generators[id].generatorStats.levelUpCost);
            }
        }

        [ContextMenu("Unlock Generator")]
        public void UnlockGenerator(int id)
        {
            if (_playerManager.Wallet.Currency < generators[id].generatorStats.levelUpCost) return;

            RemoveCurrency(generators[id].Upgrade(_playerManager.Wallet.Currency));
            generators[id].IsActive = true;
            generators[id].gameObject.SetActive(true);
        }

        [ContextMenu("Upgrade basic Generator")]
        public void UpgradeBasicGenerator()
        {
            RemoveCurrency(generators[0].Upgrade(_playerManager.Wallet.Currency));
        }
    }
}