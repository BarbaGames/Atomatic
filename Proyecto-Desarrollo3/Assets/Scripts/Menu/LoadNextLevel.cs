using UnityEngine;
using UnityEngine.SceneManagement;

namespace BarbaGames.Game.Menu
{
    public class LoadNextLevel : MonoBehaviour
    {
        private readonly int sceneId = 2;
        
        private void OnDestroy()
        {
            SceneManager.LoadScene(sceneId);
        }
    }
}