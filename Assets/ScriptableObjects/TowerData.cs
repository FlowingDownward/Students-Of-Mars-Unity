using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "Scriptable Objects/TowerData")]
public class TowerData : ScriptableObject
{
    public GameObject prefab;
    
    public float range;
    public float attackSpeed;
    public float projectileSpeed;
    public float projectileDuration;
    public float projectileSize;
    
    public bool isExplosiveProjectile;
    public float explosionDuration; 
    public float damage;
    public int price;

}
