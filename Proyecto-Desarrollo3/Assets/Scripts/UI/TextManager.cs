using UnityEngine;

using TMPro;

namespace BarbaGames.Game.Generators
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
