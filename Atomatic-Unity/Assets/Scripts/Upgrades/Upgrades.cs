using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "Upgrades")]
public class Upgrades : ScriptableObject
{
   [SerializeField] public List<Upgrade> upgrades = null;
}