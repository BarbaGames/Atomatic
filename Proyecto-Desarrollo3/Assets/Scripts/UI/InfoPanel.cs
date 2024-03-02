using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class InfoPanel : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string openTag;
        [SerializeField] private string startTag;

        private bool open = true;

        public void PlayAnimation()
        {
            _animator.SetBool(openTag, open);
            _animator.SetBool(startTag, true);
            
            
            open = !open;
            
        }
    }
}