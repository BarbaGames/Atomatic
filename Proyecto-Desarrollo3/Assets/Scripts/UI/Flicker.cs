using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class FlickerEffect : MonoBehaviour
{
    public Image flickerImage;
    public Light flickerLight;
    public float minFlickerTime = 0.1f;
    public float maxFlickerTime = 0.5f;

    private bool isFlickering = false;

    void Start()
    {
        // Start the flicker coroutine
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            // Wait for a random time between minFlickerTime and maxFlickerTime
            float flickerTime = Random.Range(minFlickerTime, maxFlickerTime);
            yield return new WaitForSeconds(flickerTime);

            // Toggle the light on and off
            isFlickering = !isFlickering;
            if ((flickerImage != null))
            {
                flickerImage.enabled = isFlickering;
            }
            if (flickerLight != null)
            {
                flickerLight.enabled = isFlickering;
            }
        }
    }
}
