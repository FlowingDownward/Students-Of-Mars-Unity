using System;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private TowerData data;
    private CircleCollider2D circleCollider;

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
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.radius = data.range;
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


    //Range Handling
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            enemiesInRange.Add(enemy);
            //Debug.Log("Enemy has entered tower range");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemiesInRange.Contains(enemy))
            {
                enemiesInRange.Remove(enemy);
                //Debug.Log("Enemy has left tower range");

            }
        }
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
