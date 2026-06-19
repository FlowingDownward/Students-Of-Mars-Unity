using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "Scriptable Objects/ProjectileData")]
public class ProjectileData : ScriptableObject
{
    public Projectile prefab;
    public int poolSize;
    public bool isFire;

    //Explosion Data
    public bool isExplosiveProjectile;
    [SerializeField] public GameObject explosionZonePrefab;
    public float explosionDuration; 
}
