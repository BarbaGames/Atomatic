using UnityEngine;

public class FaderUpdate : MonoBehaviour
{
    [SerializeField] string rtpcName;
    
    public void FaderUpdated(float sliderValue)
    {
        AkSoundEngine.SetRTPCValue(rtpcName, sliderValue);
        
        PlayerPrefs.SetFloat(rtpcName, sliderValue);
    }
}