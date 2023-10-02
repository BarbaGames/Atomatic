using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ButtonsController : MonoBehaviour
    {
        [SerializeField] private Button[] buttons;

        public void EnableButton(int id)
        {
            buttons[id].gameObject.SetActive(true);
        }

        public void DisableButton(int id)
        {
            buttons[id].gameObject.SetActive(false);
        }
    }
}