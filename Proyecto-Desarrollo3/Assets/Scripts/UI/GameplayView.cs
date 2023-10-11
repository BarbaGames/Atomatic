using System;
using System.Collections.Generic;
using System.Globalization;

using BarbaGames.Game.Animations;
using BarbaGames.Game.Generators;

using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace UI
{
    public class GameplayView : MonoBehaviour
    {
        #region EXPOSED_FIELDS
        [Header("MAIN CONFIG")]
        [SerializeField] private Button btnClick = null;
        [SerializeField] private TMP_Text txtEnergy = null;
        [SerializeField] private TMP_Text txtEnergyPerSec = null;
        [SerializeField] private GameObject flyingTextPrefab = null;
        [SerializeField] private Transform flyingTextSpawnPos = null;
        
        [Header("VIEWS")]
        [SerializeField] private GeneratorsBuyView generatorsBuyView = null;
        [SerializeField] private TooltipView tooltipView = null;
        [SerializeField] private GeneratorsView generatorsView = null;
        #endregion

        #region PRIVATE_FIELDS
        private IObjectPool<TMP_Text> flyingTextPool = null;
        #endregion

        #region PUBLIC_METHODS
        public void Init(List<GeneratorData> generatorStats, Action<string> onTryBuyGenerator, Action onPlayerClick)
        {
            generatorsBuyView.Init(generatorStats, onTryBuyGenerator, tooltipView.OnTooltipEnable, tooltipView.OnToolTipDisable);
            generatorsView.Init();
            btnClick.onClick.AddListener(onPlayerClick.Invoke);

            flyingTextPool = new ObjectPool<TMP_Text>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 10);
        }

        public void UpdateEnergy(long newEnergy)
        {
            txtEnergy.text = newEnergy.ToString(CultureInfo.InvariantCulture);
        }
        
        public void UpdateEnergyPerSec(long newEnergy)
        {
            txtEnergyPerSec.text = newEnergy.ToString(CultureInfo.InvariantCulture) + " E/s";
        }

        public void UpdateGenerator(GeneratorData generatorData)
        {
            generatorsBuyView.UpdateGenerator(generatorData);
            generatorsView.UpdateGenerator(generatorData);
        }

        public void UnlockGenerator(GeneratorData generatorData)
        {
            generatorsView.AddGenerator(generatorData, tooltipView.OnTooltipEnable, tooltipView.OnToolTipDisable);
        }

        public void SpawnFlyingText(long energyGenerated)
        {
            var text = flyingTextPool.Get();
            text.text = energyGenerated.ToString(CultureInfo.InvariantCulture);
            text.transform.parent.position = flyingTextSpawnPos.position;
        }

        private void OnFlyingTextFinish(TMP_Text tmpText)
        {
            flyingTextPool.Release(tmpText);
        }
        #endregion

        #region POOLING
        TMP_Text CreatePooledItem()
        {
            GameObject go = Instantiate(flyingTextPrefab);
            TMP_Text text = go.GetComponentInChildren<TMP_Text>();
            AnimationController anim = go.GetComponentInChildren<AnimationController>();
            anim.SetCallBack(OnFlyingTextFinish);
            return text;
        }

        // Called when an item is returned to the pool using Release
        void OnReturnedToPool(TMP_Text tmpText)
        {
            tmpText.gameObject.SetActive(false);
        }

        // Called when an item is taken from the pool using Get
        void OnTakeFromPool(TMP_Text tmpText)
        {
            tmpText.gameObject.SetActive(true);
        }

        // If the pool capacity is reached then any items returned will be destroyed.
        // We can control what the destroy behavior does, here we destroy the GameObject.
        void OnDestroyPoolObject(TMP_Text tmpText)
        {
            Destroy(tmpText.gameObject);
        }
        #endregion
    }
}