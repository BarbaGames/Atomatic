using System;

using Newtonsoft.Json;

using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade")]
[Serializable]
public class Upgrade : ScriptableObject
{
    [SerializeField] public string description;
    [JsonIgnore] public Sprite icon;
    [SerializeField] public long price;
    [SerializeField] public int id;
    [SerializeField] public bool bought;
}