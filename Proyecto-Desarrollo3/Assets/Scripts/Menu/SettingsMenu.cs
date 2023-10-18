using System.Collections.Generic;
using System.Linq;
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
            Resolution[] auxResolutions = Screen.resolutions;
            resolutions = new Resolution[auxResolutions.Length];
            resDropdown.ClearOptions();

            List<string> resOptions = new List<string>();
            Dictionary<string, int> uniqueResolutions = new Dictionary<string, int>();
            int i = 0;
            foreach (var resolution in auxResolutions)
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
                    resolutions[i] = resolution;
                    i++;
                }
                if (resolution.width == Screen.width && resolution.height == Screen.height)
                {
                    resID = uniqueResolutions.Count - 1;
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
