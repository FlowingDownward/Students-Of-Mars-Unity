using UnityEngine;

public class Spawner : MonoBehaviour
{
    private float spawnTimer;
    public float spawnInterval = 3f;
    public GameObject EnemyPrefab;

    [SerializeField] private ObjectPooler pool;

    // Update is called once per frame
    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            spawnTimer = spawnInterval;
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        GameObject spawnedObject = pool.GetPooledObject();
        spawnedObject.transform.position = transform.position;
        spawnedObject.SetActive(true);
    }
}
