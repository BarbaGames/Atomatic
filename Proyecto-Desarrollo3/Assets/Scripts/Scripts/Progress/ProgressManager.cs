using UnityEngine;

namespace Code.Scripts.Progress
{
    public class ProgressManager : MonoBehaviour
    {
        private LoadManager _loadManager;
        private SaveManager _saveManager;

        private void Awake()
        {
            _loadManager = new LoadManager();
            _saveManager = new SaveManager();
        }
    }
}