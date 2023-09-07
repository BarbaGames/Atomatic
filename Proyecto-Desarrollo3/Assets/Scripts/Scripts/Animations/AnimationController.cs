using UnityEngine;

namespace Code.Scripts.Animations
{
    public class AnimationController : MonoBehaviour
    {
        public void SetParentInactive()
        {
            GameObject parent = gameObject.transform.parent.gameObject;
            parent.SetActive(false);
            
        }
    }
}