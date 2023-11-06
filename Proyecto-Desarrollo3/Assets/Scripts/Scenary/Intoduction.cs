using UnityEngine;

namespace Scenary
{
    public class Intoduction : MonoBehaviour
    {
        [SerializeField] private GameObject[] pages;

        public void ActivateNext()
        {
            foreach (GameObject page in pages)
            {
                if (!page.activeSelf)
                {
                    page.SetActive(true);
                    return;
                }
            }
            gameObject.SetActive(false);
        }
    }
}