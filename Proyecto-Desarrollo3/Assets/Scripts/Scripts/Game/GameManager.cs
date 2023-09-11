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
        [SerializeField] private GameObject textHolderPrefab;
        [SerializeField] private Button[] generatorButtons;

        private const short TextHolderAmount = 15;
        public static GameObject[] textHolderPool;
        private PlayerController _playerController;
        private PlayerView _playerView;
        private const string CurrencyKey = "currency";

        public delegate void CurrencyEventHandler(double value);
        public static event CurrencyEventHandler OnCurrencyEvent;
        public delegate void GeneratorUpgradeEventHandler(short id, int cost);
        public static event GeneratorUpgradeEventHandler OnGeneratorChangeEvent;

        private void Awake()
        {
            InitPlayerComponents();
            if (FileHandler.FileExist(CurrencyKey))
            {
                if (FileHandler.TryLoadFileRaw(CurrencyKey, out string data))
                {
                    //AddCurrency(int.Parse(data));
                }
            }
            
            // if (PlayerPrefs.HasKey(CurrencyKey))
            // {
            //     AddCurrency(PlayerPrefs.GetInt(CurrencyKey));
            // }

            textHolderPool = new GameObject[TextHolderAmount];

            for(int i = 0; i < textHolderPool.Length; i++)
            {
                textHolderPool[i] = Instantiate(textHolderPrefab);
                textHolderPool[i].SetActive(false);
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
        }

        private void InitPlayerComponents()
        {
            _playerController = gameObject.AddComponent<PlayerController>();
            _playerController.Wallet = gameObject.AddComponent<Wallet>();

            _playerView = gameObject.AddComponent<PlayerView>();
            _playerView.CurrencyText = currencyText;
            _playerView.wallet = _playerController.Wallet;

            _playerView.generatorsUiText = new TMP_Text[generatorButtons.Length];

            for (int i = 0; i < generatorButtons.Length; i++)
            {
                _playerView.generatorsUiText[i] = generatorButtons[i].GetComponentsInChildren<TMP_Text>()[0];
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
        }

        public void UpgradeLevel(int id)
        {
            if (!generators[id]) return;
            if(_playerController.Wallet.Currency < generators[id].generatorStats.levelUpCost) return;
            
            if (!generators[id].IsActive)
            {
                UnlockGenerator(id);
            }
            else
            {
                RemoveCurrency(generators[id].Upgrade(_playerController.Wallet.Currency));
                OnGeneratorChangeEvent?.Invoke((short)id, (int)generators[id].generatorStats.levelUpCost);
            }
        }

        [ContextMenu("Unlock Generator")]
        public void UnlockGenerator(int id)
        {
            if (_playerController.Wallet.Currency < generators[id].generatorStats.levelUpCost) return;

            RemoveCurrency(generators[id].Upgrade(_playerController.Wallet.Currency));
            generators[id].IsActive = true;
            generators[id].gameObject.SetActive(true);
        }
    }
}