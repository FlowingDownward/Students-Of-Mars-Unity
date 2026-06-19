using System;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private TowerData data;
    [SerializeField] private CircleCollider2D rangeCollider;

    public TowerData Data => data;

    private List<Enemy> enemiesInRange;
    private ObjectPooler projectilePool;
    private GameObject rangeIndicator;
    private ProjectileData currentProjectile;

    private float shootTimer;
    private int killCount = 0;
    public int KillCount => killCount;

    public static event Action<int> OnKillCountChanged;

    public bool isbeingViewed = false;

    private void OnEnable()
    {
        Enemy.OnEnemyDestroyed += HandleEnemyDestroyed;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDestroyed -= HandleEnemyDestroyed;
    }

    private void Start()
    {
        rangeCollider.radius = data.range;
        enemiesInRange = new List<Enemy>();
        projectilePool = GetComponent<ObjectPooler>();
        currentProjectile = data.projectileFired;
        shootTimer = data.attackSpeed;
    }

    private void Update()
    {
        shootTimer -= Time.deltaTime;
        if(shootTimer <= 0)
        {
            shootTimer = data.attackSpeed;
            Fire();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, data.range*.3f);
    }

    public void AddEnemy(Enemy enemy)
    {
        if (!enemiesInRange.Contains(enemy))
        {
            enemiesInRange.Add(enemy);
        }
    }

    public void RemoveEnemy(Enemy enemy)
    {
        enemiesInRange.Remove(enemy);
    }
    

    // Targeting and Shooting
    private void Fire()
    {
        if (enemiesInRange.Count > 0)
        {
            //Debug.Log("Tower is attempting to fire");

            Projectile projectile = ProjectilePoolManager.Instance.GetProjectile(data.projectileFired.prefab);
            projectile.transform.position = transform.position;
            Vector2 shootDirection = (enemiesInRange[0].transform.position - transform.position).normalized;
            projectile.Shoot(data, currentProjectile, shootDirection, this);
        }
    }

    public void RegisterKill()
    {
        killCount++;
        Debug.Log($"{name} has {killCount} kills");
        if (isbeingViewed == true)
        {
            OnKillCountChanged?.Invoke(killCount);
        }
    }

    private void HandleEnemyDestroyed(Enemy enemy)
    {
        enemiesInRange.Remove(enemy);
    }

    public void SetProjectile(Projectile newProjectile)
    {
        //currentProjectile = newProjectile;
    }

}
