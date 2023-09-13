using System;

using UnityEngine;

using TMPro;

namespace BarbaGames.Game.Animations
{
    public class AnimationController : MonoBehaviour
    {
        private Action<TMP_Text> onFinish = null;
        
        public void SetCallBack(Action<TMP_Text> onFinish)
        {
            this.onFinish = onFinish;
        }
        
        public void SetInactive()
        {
            onFinish.Invoke(GetComponent<TMP_Text>());
        }
    }
}