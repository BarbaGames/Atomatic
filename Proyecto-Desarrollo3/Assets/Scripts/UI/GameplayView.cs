using System;
using System.Collections.Generic;
using System.Globalization;

using BarbaGames.Game.Animations;

using Generators;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
        [SerializeField] private Transform flyingTextSpawnPosSmaller = null;
        [SerializeField] private Transform flyingTextSpawnPosBigger = null;
        
        [Header("VIEWS")]
        [SerializeField] private GeneratorsBuyView generatorsBuyView = null;
        [SerializeField] private TooltipView tooltipView = null;
        [SerializeField] private GeneratorsView generatorsView = null;
        [SerializeField] private UpgradeBuyView upgradeBuyView = null;
        #endregion

        #region PRIVATE_FIELDS
        private IObjectPool<TMP_Text> flyingTextPool = null;
        #endregion

        #region PUBLIC_METHODS
        public void Init(List<GeneratorData> generatorStats, List<Upgrade> upgrades, Action<string> onTryBuyGenerator,Action<int> onTryBuyUpgrade, Action onPlayerClick)
        {
            generatorsBuyView.Init(generatorStats, onTryBuyGenerator, tooltipView.OnTooltipEnable, tooltipView.OnToolTipDisable);
            generatorsView.Init();
            upgradeBuyView.Init(upgrades, onTryBuyUpgrade, tooltipView.OnTooltipEnable, tooltipView.OnToolTipDisable);
            btnClick.onClick.AddListener(onPlayerClick.Invoke);

            flyingTextPool = new ObjectPool<TMP_Text>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 10);
        }

        public void UpdateEnergy(long newEnergy)
        {
            txtEnergy.text = newEnergy.ToString("N0");
        }
        
        public void UpdateEnergyPerSec(long newEnergy)
        {
            txtEnergyPerSec.text = newEnergy.ToString("N0") + " E/s";
        }

        public void UpdateGenerator(GeneratorData generatorData)
        {
            generatorsBuyView.UpdateGenerator(generatorData);
            generatorsView.UpdateGenerator(generatorData);
        }

        public void UpdateUpgrade(Upgrade upgrade)
        {
            upgradeBuyView.UpdateUpgrade(upgrade);
        }

        public void UnlockGenerator(GeneratorData generatorData)
        {
            generatorsView.AddGenerator(generatorData, tooltipView.OnTooltipEnable, tooltipView.OnToolTipDisable);
        }

        public void SpawnFlyingText(long energyGenerated)
        {
            var text = flyingTextPool.Get();
            text.text = energyGenerated.ToString(CultureInfo.InvariantCulture);
            var position = flyingTextSpawnPosSmaller.position;
            Vector3 pos = new Vector3( 0,0,position.z);
            var position1 = flyingTextSpawnPosBigger.position;
            pos.x = Random.Range(position.x, position1.x);
            pos.y = Random.Range(position.y, position1.y);
            text.transform.parent.position = pos;
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