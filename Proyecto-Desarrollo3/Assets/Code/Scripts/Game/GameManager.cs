using TMPro;
using UnityEngine;

namespace Code.Scripts.Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text currency;
        private int intValue = 10;
        public delegate void DoubleValueEventHandler(double value);
        public static event DoubleValueEventHandler OnValueReceived;

        
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
        
        public void SendDoubleValue(double valueToSend)
        {
                OnValueReceived?.Invoke(valueToSend);
        }
        public void SendFloatValue(float valueToSend)
        {
            OnValueReceived?.Invoke(valueToSend);
        }

        private void CheckClick()
        {
            Ray ray = Camera.main!.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject)
                {
                    Debug.Log("Object Clicked: " + gameObject.name);
                    if (gameObject.GetComponent<Clickable>())
                    {
                        SendFloatValue(Player.UserLvl);
                    }
                }
            }
        }
        
        private void UpdateCurrency(double value)
        {
            currency.text = value.ToString();
        }

        public void UpgradeLevel()
        {
            Player.UserLvl++;
        }

        [ContextMenu("Send 10 coins")]
        private void Send10Coins()
        {
            OnValueReceived?.Invoke(intValue);
        }
    }
}
