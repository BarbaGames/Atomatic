using System;

using Newtonsoft.Json;

using Progress;

using UnityEngine;

namespace Scenary
{
    public class Intoduction : MonoBehaviour
    {
        [SerializeField] private GameObject[] pages;
        
        private const string IntroductionKey = "watchedIntro";

        public void Start()
        {
            if (FileHandler.TryLoadFileRaw(IntroductionKey, out string introDataString))
            {
                bool watchedIntro = JsonConvert.DeserializeObject<bool>(introDataString);

                if (watchedIntro)
                {
                    gameObject.SetActive(false);
                }
            }
        }

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
            
            FileHandler.SaveFile(IntroductionKey,JsonConvert.SerializeObject(true));
            
            gameObject.SetActive(false);
        }
    }
}