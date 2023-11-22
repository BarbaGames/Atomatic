using System;

using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade")]
[Serializable]
public class Upgrade : ScriptableObject
{
    [SerializeField] public string description;
    [SerializeField] public Sprite icon;
    [SerializeField] public long price;
    [SerializeField] public int id;
    [SerializeField] public bool bought;
    [SerializeField] public int generatorId;
    [SerializeField] public int currencyGeneratedAmount;
    [SerializeField] public bool stageUpgrade;
    [SerializeField] public int stageUnlock;
}