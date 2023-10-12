using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class SettingsMenu : MonoBehaviour
    {

        public Dropdown resDropdown;
        private Resolution[] resolutions;
        private void Start()
        {
            Init();
        }

        public void SetResolution(int resolutionId)
        {
            Resolution res = resolutions[resolutionId];
            Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        }
        
        public void SetFullscreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
        }

        public void SetQuality(int qualityId)
        {
            QualitySettings.SetQualityLevel(qualityId);
        }
        
        private void Init()
        {
            int resID = 0;
            resolutions = Screen.resolutions;
            
            resDropdown.ClearOptions();
            
            List<string> resOptions = new List<string>();

            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;
                resOptions.Add(option);

                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height)
                {
                    resID = i;
                }
            }
            
            resDropdown.AddOptions(resOptions);
            resDropdown.value = resID;
            resDropdown.RefreshShownValue();
        }
    }
}
