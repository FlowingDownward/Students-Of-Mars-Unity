using System.Collections.Generic;
using UnityEngine;

public class ProjectilePoolManager : MonoBehaviour
{
    public static ProjectilePoolManager Instance;

    private Dictionary<Projectile, Queue<Projectile>> pools =
        new Dictionary<Projectile, Queue<Projectile>>();

    [SerializeField] private int defaultPoolSize = 20;

    private void Awake()
    {
        Instance = this;
    }

    public Projectile GetProjectile(Projectile prefab)
    {
        if (!pools.ContainsKey(prefab))
        {
            CreatePool(prefab);
        }

        Queue<Projectile> pool = pools[prefab];

        if (pool.Count == 0)
        {
            ExpandPool(prefab);
        }

        Projectile projectile = pool.Dequeue();

        projectile.gameObject.SetActive(true);

        return projectile;
    }

    public void ReturnProjectile(Projectile projectile, Projectile prefab)
    {
        projectile.gameObject.SetActive(false);

        pools[prefab].Enqueue(projectile);
    }

    private void CreatePool(Projectile prefab)
    {
        pools.Add(prefab, new Queue<Projectile>());

        ExpandPool(prefab);
    }

    private void ExpandPool(Projectile prefab)
    {
        Queue<Projectile> pool = pools[prefab];

        for (int i = 0; i < defaultPoolSize; i++)
        {
            Projectile p = Instantiate(prefab);

            p.gameObject.SetActive(false);

            pool.Enqueue(p);
        }
    }
}