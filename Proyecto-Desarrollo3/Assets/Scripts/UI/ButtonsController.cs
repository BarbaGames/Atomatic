using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ButtonsController : MonoBehaviour
    {
        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private Upgrade[] upgrades;
        public static Action<long, int> onUpgradeUnlocked;
        private GameObject[] buttons;
        
        private void Start()
        {
            buttons = new GameObject[upgrades.Length];
            for (int i = 0; i < upgrades.Length ; i++)
            {
                var instance = Instantiate(buttonPrefab, gameObject.transform);
                
                instance.GetComponentsInChildren<Image>()[1].sprite = upgrades[i].icon;
                instance.GetComponent<Button>().onClick.AddListener(()=>onUpgradeUnlocked(upgrades[i].price, upgrades[i].id));
                
                buttons[i] = instance;
            }

        }

        private void OnEnable()
        {
            onUpgradeUnlocked += DisableButton;

        }

        private void OnDisable()
        {
            onUpgradeUnlocked -= DisableButton;

        }

        public void EnableButton(int id)
        {
            buttons[id].gameObject.SetActive(true);
        }

        public void DisableButton(long price, int id)
        {
            buttons[id].SetActive(false);
            buttons[id].GetComponent<Button>().onClick.RemoveListener(()=>onUpgradeUnlocked(1,1));
        }
    }
}