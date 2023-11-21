using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Upgrades")]
public class Upgrades : ScriptableObject
{
   [SerializeField] public List<Upgrade> upgrades = null;
}