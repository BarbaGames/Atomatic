using UnityEngine;

namespace BarbaGames.Game.Animations
{
    public class AnimationController : MonoBehaviour
    {
        public void SetParentInactive()
        {
            gameObject.SetActive(false);
        }
    }
}