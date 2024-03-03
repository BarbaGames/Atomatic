using System.Collections.Generic;
using UnityEngine;

namespace Generators
{
    [CreateAssetMenu(fileName = "Generators")]
    public class GeneratorSO : ScriptableObject
    {
        [SerializeField] public List<GeneratorData> generators = null;
    }
}
