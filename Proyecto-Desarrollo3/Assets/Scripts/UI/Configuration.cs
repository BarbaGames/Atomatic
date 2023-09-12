using UnityEngine;

namespace BarbaGames.Game.UI
{
    [CreateAssetMenu(fileName = "Configuration")]
    public class Configuration : ScriptableObject
    {
        public string description;
        public Sprite icon;
    }
}