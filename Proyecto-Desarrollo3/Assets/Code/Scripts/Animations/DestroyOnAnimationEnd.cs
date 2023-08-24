using UnityEngine;

namespace Code.Scripts.Animations
{
    public class DestroyOnAnimationEnd : MonoBehaviour
    {
        public void DestroyParent()
        {
            GameObject parent = gameObject.transform.parent.gameObject;
            Destroy(parent);
            
        }
    }
}