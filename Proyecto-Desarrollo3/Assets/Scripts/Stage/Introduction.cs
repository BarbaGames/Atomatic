using UnityEngine;

namespace Stage
{
    public class Introduction : MonoBehaviour
    {
        [SerializeField] private GameObject[] panels;
        [SerializeField] private GameObject[] pages;
        private int _currentPage = 0;
        
        public void ActivateNextPage()
        {
            for (int i = 0; i < panels.Length; i++)
            {
                if (!panels[i].activeSelf)
                {
                    panels[i].SetActive(true);
                    
                    if(i % 4 == 0) ActivateNextPanel();
                    
                    return;
                }
            }
            
            gameObject.SetActive(false);
        }

        private void ActivateNextPanel()
        {
            pages[_currentPage].SetActive(false);
            _currentPage++;
            pages[_currentPage].SetActive(true);
        }
    }
}