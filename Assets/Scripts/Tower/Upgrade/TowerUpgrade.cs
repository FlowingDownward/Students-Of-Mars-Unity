using UnityEngine;

[System.Serializable]
public class TowerUpgrade
{
    public string upgradeName;

    public int cost;
    // sprite

    public StatModifier[] modifiers;
    public UpgradeEffect[] effects;
}