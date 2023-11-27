using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.PostEvent("StartMusic", gameObject);
        ChangeScene(SceneManager.GetActiveScene().name);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Awake()
    {
        AudioManager[] duplicates = GameObject.FindObjectsOfType<AudioManager>();
        if (duplicates.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public static void ChangeScene(string scene)
    {
        Debug.Log(scene);
        switch (scene)
        {
            case "Menu":
                AkSoundEngine.SetState("GameScene", "MainMenu");
                break;
            case "Tutorial":
            case "Game":
                AkSoundEngine.SetState("GameScene", "InGame");
                break;
            case "Win":
                AkSoundEngine.SetState("GameScene", "Win");
                break;
            default:
                Debug.LogWarning("La escena " + scene.ToString() + " aún no tiene implementado un cambio de música, ¡notificar a los de audio!");
                break;


        }
    }

    public static void SetGenerator(int generator)
    {
        string generatorState = "";
        switch (generator)
        {
            case 0: generatorState = "One"; break;
            case 1: generatorState = "Two"; break;
            case 2: generatorState = "Three"; break;
        }
        AkSoundEngine.SetState("CurrentGenerator", generatorState);
    }
}
