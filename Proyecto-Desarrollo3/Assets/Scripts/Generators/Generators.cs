using System.Collections.Generic;

using UnityEngine;

namespace BarbaGames.Game.Generators
{
    [CreateAssetMenu(fileName = "Generators")]
    public class Generators : ScriptableObject
    {
        [SerializeField] public List<GeneratorData> generators = null;
    }
}
