using UnityEngine;
using UnityEngine.SceneManagement;

namespace BarbaGames.Game.Menu
{
    public class MenuManager : MonoBehaviour
    {
        private const string SceneName = "Tutorial";

        /// <summary>
        /// Loads the game scene
        /// </summary>
        public void LoadGame()
        {
            LoadSceneByName();
        }

        /// <summary>
        /// Quits the application
        /// </summary>
        public void OnExitButtonClick()
        {
            Application.Quit();
        }

        /// <summary>
        /// Loads the required scene from the current scene.
        /// </summary>
        private void LoadSceneByName()
        {
            SceneManager.LoadScene(SceneName);
        }
    }
}