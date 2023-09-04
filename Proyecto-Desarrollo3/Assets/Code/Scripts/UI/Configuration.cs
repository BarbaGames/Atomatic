using UnityEngine;

namespace Code.Scripts.UI
{
    [CreateAssetMenu(fileName = "Configuration")]
    public class Configuration : ScriptableObject
    {
        public string description;
        public Sprite icon;
    }
}