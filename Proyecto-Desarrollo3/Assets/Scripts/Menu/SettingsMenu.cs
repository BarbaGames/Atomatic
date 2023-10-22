using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField] private Toggle fullscreenToggle;
        public Dropdown resDropdown;
        private const string FullscreenKey = "fullscreen";
        private const string ResWidthKey = "width";
        private const string ResHeightKey = "height";
        private Resolution[] resolutions;
        private void Start()
        {
            Init();
        }

        public void SetResolution(int resolutionId)
        {
            Resolution res = resolutions[resolutionId];
            Screen.SetResolution(res.width, res.height, Screen.fullScreen);
            PlayerPrefs.SetInt(ResWidthKey, res.width);
            PlayerPrefs.SetInt(ResHeightKey, res.height);
        }
        
        public void SetFullscreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
            PlayerPrefs.SetInt(FullscreenKey, isFullscreen ? 1 : 0);
        }

        public void SetQuality(int qualityId)
        {
            QualitySettings.SetQualityLevel(qualityId);
        }
        
        private void Init()
        {
            InitResolutionDropdown();
            InitSettingsValues();
        }

        private void InitResolutionDropdown()
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

        void InitSettingsValues()
        {
            if (PlayerPrefs.HasKey(FullscreenKey))
            {
                Screen.fullScreen = PlayerPrefs.GetInt(FullscreenKey) == 1;
            }
            else
            {
                PlayerPrefs.SetInt(FullscreenKey, Screen.fullScreen ? 1 : 0);
            }

            fullscreenToggle.isOn = Screen.fullScreen;
            if (PlayerPrefs.HasKey(ResWidthKey) && PlayerPrefs.HasKey(ResHeightKey))
            {
                Screen.SetResolution(PlayerPrefs.GetInt(ResWidthKey), PlayerPrefs.GetInt(ResHeightKey), Screen.fullScreen);
            }
            else
            {
                PlayerPrefs.SetInt(ResWidthKey, Screen.currentResolution.width);
                PlayerPrefs.SetInt(ResHeightKey, Screen.currentResolution.height);
            }
        }
    }
}
