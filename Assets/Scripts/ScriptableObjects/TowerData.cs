using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "Scriptable Objects/TowerData")]
public class TowerData : ScriptableObject
{
    public GameObject prefab;
    public ProjectileData projectileFired;

    public string towerName;
    
    public float range;
    public float attackSpeed;
    public float projectileSpeed;
    public float projectileDuration;
    public float projectileSize;
    
    
    public float damage;
    public int price;

    public TowerUpgrade[] upgradePathOne;
    public TowerUpgrade[] upgradePathTwo;

}
