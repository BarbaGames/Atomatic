using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Menu
{
    public class VolumeSlider : MonoBehaviour
    {
        [SerializeField] private AudioMixer mainMixer;
        [SerializeField] private Slider volumeSlider;

        private const string MixerVolume = "Volume";
        
        private void Start()
        {
            mainMixer.GetFloat(MixerVolume, out float currentVolume);
            volumeSlider.value = currentVolume;
        }
        
        /// <summary>
        /// Changes the game's volume
        /// </summary>
        /// <param name="volume"> new volume </param>
        public void SetVolume(float volume)
        {
            mainMixer.SetFloat("Volume", Mathf.Log10(volume) * 20); 
        }
    }
}
