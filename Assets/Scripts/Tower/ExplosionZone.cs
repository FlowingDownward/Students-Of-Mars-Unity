using System.Collections.Generic;
using UnityEngine;

public class ExplosionZone : MonoBehaviour
{
    private float damagePerTick;
    private float duration;
    private float tickRate = 0.2f;

    private float tickTimer;

    private List<Enemy> enemiesInZone = new List<Enemy>();

    public void Initialize(float damage, float lifeTime)
    {
        damagePerTick = damage;
        duration = lifeTime;
        tickTimer = tickRate;
    }

    private void Update()
    {
        duration -= Time.deltaTime;

        if (duration <= 0)
        {
            Destroy(gameObject);
            return;
        }

        tickTimer -= Time.deltaTime;

        if (tickTimer <= 0f)
        {
            DealDamage();
            tickTimer = tickRate;
        }
    }

    private void DealDamage()
    {
        enemiesInZone.RemoveAll(e => e == null);

        foreach (var enemy in enemiesInZone)
        {
            enemy.TakeDamage(damagePerTick);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            if (enemy != null && !enemiesInZone.Contains(enemy))
                enemiesInZone.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            if (enemy != null)
                enemiesInZone.Remove(enemy);
        }
    }
}