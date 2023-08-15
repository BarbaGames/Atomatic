using System;
using UnityEngine;

namespace Code.Scripts.Game
{
    public class InputManager : MonoBehaviour
    {
        public static Action OnClickEvent; 
        private void OnClick()
        {
            OnClickEvent?.Invoke();
        }
    }
}
