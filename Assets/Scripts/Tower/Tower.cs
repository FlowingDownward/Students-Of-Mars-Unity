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

    private float shootTimer;

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

            GameObject projectile = projectilePool.GetPooledObject();
            projectile.transform.position = transform.position;
            projectile.SetActive(true);
            Vector2 shootDirection = (enemiesInRange[0].transform.position - transform.position).normalized;
            projectile.GetComponent<Projectile>().Shoot(data, shootDirection);
        }
    }

    private void HandleEnemyDestroyed(Enemy enemy)
    {
        enemiesInRange.Remove(enemy);
    }

}
