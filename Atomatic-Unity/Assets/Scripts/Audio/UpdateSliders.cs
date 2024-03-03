using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Audio
{
    public class UpdateSliders : MonoBehaviour
    {
        [SerializeField] private Slider masterSlider;
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private Slider musicSlider;
        private const string MasterKey = "MasterVol";
        private const string SfxKey = "SfxVol";
        private const string MusicKey = "MusVol";
        private void Awake()
        {
           
            masterSlider.value = PlayerPrefs.GetFloat(MasterKey);
            
            sfxSlider.value = PlayerPrefs.GetFloat(SfxKey);
            
            musicSlider.value = PlayerPrefs.GetFloat(MusicKey);
        }
    }
}