using System;

using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text versionText = null;
        private const string SceneName = "Game";


        public void Start()
        {
            versionText.text = "v"+Application.version;
        }
        /// <summary>
        /// Quits the application
        /// </summary>
        public void QuitApplication()
        {
            Application.Quit();
        }

        /// <summary>
        /// Loads the scene saved in SceneName.
        /// </summary>
        public void LoadSceneByName()
        {
            SceneManager.LoadScene(SceneName);
            AudioManager.ChangeScene(SceneName);
        }
        
        /// <summary>
        /// Loads the required scene by the Id introduced.
        /// </summary>
        public void LoadSceneById(int id)
        {
            SceneManager.LoadScene(id);
        }
    }
}