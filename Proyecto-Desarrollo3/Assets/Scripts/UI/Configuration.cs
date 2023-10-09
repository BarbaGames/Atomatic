using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "Configuration")]
    public class Configuration : ScriptableObject
    {
        public string description;
        public Sprite icon;
        public long price;
        public int id;
        public bool bought;
    }
}