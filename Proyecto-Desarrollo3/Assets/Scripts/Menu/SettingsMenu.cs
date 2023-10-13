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
            List<Resolution> uniqueResolutions = new List<Resolution>();

            foreach (Resolution res in resolutions)
            {
                bool isDuplicate = false;
                Resolution currentRes = res;

                foreach (Resolution uniqueRes in uniqueResolutions)
                {
                    if (uniqueRes.width == currentRes.width && uniqueRes.height == currentRes.height)
                    {
                        isDuplicate = true;
                        break;
                    }
                }

                if (!isDuplicate)
                {
                    uniqueResolutions.Add(currentRes);

                    string option = currentRes.width + " x " + currentRes.height + " @ " + currentRes.refreshRate + " Hz";
                    resOptions.Add(option);

                    if (currentRes.width == Screen.width && currentRes.height == Screen.height)
                    {
                        resID = uniqueResolutions.Count - 1;
                    }
                }
            }

            resDropdown.AddOptions(resOptions);
            resDropdown.value = resID;
            resDropdown.RefreshShownValue();
        }
    }
}
