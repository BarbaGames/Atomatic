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
            Dictionary<string, int> uniqueResolutions = new Dictionary<string, int>();

            foreach (var resolution in resolutions)
            {
                string resolutionString = resolution.width + " x " + resolution.height;
                int refreshRate = (int)resolution.refreshRateRatio.value;

                if (uniqueResolutions.ContainsKey(resolutionString))
                {
                    if (refreshRate > uniqueResolutions[resolutionString])
                    {
                        uniqueResolutions[resolutionString] = refreshRate;
                    }
                }
                else
                {
                    uniqueResolutions.Add(resolutionString, refreshRate);
                }
            }

            foreach (var kvp in uniqueResolutions)
            {
                string option = kvp.Key + " @ " + kvp.Value + " Hz";
                resOptions.Add(option);
            }

            resDropdown.AddOptions(resOptions);
            resDropdown.value = resID;
            resDropdown.RefreshShownValue();
        }

    }
}
