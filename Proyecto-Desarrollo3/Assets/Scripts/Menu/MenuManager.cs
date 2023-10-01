using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MenuManager : MonoBehaviour
    {
        private const string SceneName = "Game";

        /// <summary>
        /// Quits the application
        /// </summary>
        public void OnExitButtonClick()
        {
            Application.Quit();
        }

        /// <summary>
        /// Loads the scene saved in SceneName.
        /// </summary>
        public void LoadSceneByName()
        {
            SceneManager.LoadScene(SceneName);
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