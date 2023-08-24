using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Scripts.Generators
{
    public class TextManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        public void SetText(string newText)
        {
            text.text = newText;
        }
    }
}
