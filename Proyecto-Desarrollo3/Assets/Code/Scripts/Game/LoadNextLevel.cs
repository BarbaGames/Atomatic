using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
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