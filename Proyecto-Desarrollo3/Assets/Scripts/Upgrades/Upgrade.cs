using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade")]
public class Upgrade : ScriptableObject
{
    public string description;
    public Sprite icon;
    public long price;
    public int id;
    public bool bought;
}