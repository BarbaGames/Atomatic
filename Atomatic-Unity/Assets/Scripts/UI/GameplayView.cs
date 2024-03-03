using System;
using System.Collections.Generic;
using System.Globalization;

using BarbaGames.Game.Animations;

using Generators;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
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
        [SerializeField] private Transform flyingTextSpawnBotCorner = null;
        [SerializeField] private Transform flyingTextSpawnTopCorner = null;
        
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
        public void Init(List<Upgrade> upgrades, Action<int> onTryBuyUpgrade, Action onPlayerClick)
        {
            generatorsView.Init();
            upgradeBuyView.Init(upgrades, onTryBuyUpgrade, tooltipView.OnTooltipEnable, tooltipView.OnToolTipDisable);
            btnClick.onClick.AddListener(onPlayerClick.Invoke);

            flyingTextPool = new ObjectPool<TMP_Text>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 10);
        }

        public void InitGeneratorsBuyView(List<GeneratorData> generatorStats, Action<int> onTryBuyGenerator, bool newGame)
        {
            generatorsBuyView.Init(generatorStats, onTryBuyGenerator, tooltipView.OnTooltipEnable, tooltipView.OnToolTipDisable, newGame);
        }

        public void UpdateEnergy(long newEnergy)
        {
            txtEnergy.text = newEnergy.ToString("N0");
            generatorsBuyView.OnEnergyUpdate(newEnergy);
        }
        
        public void UpdateEnergyPerSec(long newEnergy)
        {
            txtEnergyPerSec.text = newEnergy.ToString("N0") + " E/s";
        }

        public void UpdateGenerator(GeneratorData generatorData, bool fromSave)
        {
            generatorsBuyView.UpdateGenerator(generatorData);
            generatorsView.UpdateGenerator(generatorData, fromSave);
        }
        
        public void UpdateUpgrade(Upgrade upgrade)
        {
            upgradeBuyView.UpdateUpgrade(upgrade);
        }
        
        public bool UnlockUpgrade(Upgrade upgrade)
        {
           return upgradeBuyView.UnlockUpgrade(upgrade);
        }
        
        public void UnlockUpgrade(GeneratorData generatorData)
        {
            upgradeBuyView.UnlockUpgrade(generatorData);
        }

        public void UnlockGenerator(GeneratorData generatorData)
        {
            if(generatorData.background == null) return;
            generatorsView.AddGenerator(generatorData, tooltipView.OnTooltipEnable, tooltipView.OnToolTipDisable);
        }

        public void AddGenerator(GeneratorData generatorData)
        {
            generatorsBuyView.AddGenerator(generatorData);
        }

        public void SpawnFlyingText(long energyGenerated)
        {
            var text = flyingTextPool.Get();
            text.text = energyGenerated.ToString(CultureInfo.InvariantCulture);
            var position = flyingTextSpawnBotCorner.position;
            Vector3 pos = new Vector3( 0,0,position.z);
            var position1 = flyingTextSpawnTopCorner.position;
            pos.x = Random.Range(position.x, position1.x);
            pos.y = Random.Range(position.y, position1.y);
            text.transform.parent.position = pos;
        }

        private void OnFlyingTextFinish(TMP_Text tmpText)
        {
            flyingTextPool.Release(tmpText);
        }

        public void UpdateToolTip(GeneratorData generatorData)
        {
            tooltipView.UpdateToolTip(generatorData);
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