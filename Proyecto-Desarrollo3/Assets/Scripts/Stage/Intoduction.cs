using Newtonsoft.Json;
using Progress;
using UnityEngine;
using UnityEngine.Serialization;

namespace Stage
{
    public class Intoduction : MonoBehaviour
    {
        [SerializeField] private GameObject[] panels;
        [SerializeField] private GameObject[] pages;
        [SerializeField] private bool isEnding = false;
        private int _currentPage = 0;
        private const string IntroductionKey = "watchedIntro";

        public void Start()
        {
            if (FileHandler.TryLoadFileRaw(IntroductionKey, out string introDataString))
            {
                bool watchedIntro = JsonConvert.DeserializeObject<bool>(introDataString);

                if (watchedIntro & !isEnding)
                {
                    gameObject.SetActive(false);
                }
            }
        }

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

            FileHandler.SaveFile(IntroductionKey, JsonConvert.SerializeObject(true));

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