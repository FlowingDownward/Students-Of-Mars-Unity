using System;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static event Action<int> OnWaveChanged;

    //Wave Management
    [SerializeField] private WaveData [] waves;
    private int currentWaveIndex = 0;
    private int waveCounter = 0;
    
    private WaveData CurrentWave => waves[currentWaveIndex];

    private float spawnTimer;
    private float spawnCounter;
    private int enemiesRemoved;

    private bool isWaveActive = false;
    private bool waitingForNextWave = true;

    //Enemy Pools    
    [SerializeField] private ObjectPooler dronePool;
    [SerializeField] private ObjectPooler speederPool;
    [SerializeField] private ObjectPooler tankPool;

    private Dictionary<EnemyType, ObjectPooler> poolDictionary;

    private void Awake()
    {
        poolDictionary = new Dictionary<EnemyType,ObjectPooler>()
        {
            {EnemyType.Drone, dronePool},
            {EnemyType.Speeder, speederPool},
            {EnemyType.Tank, tankPool},
        };
    }

    private void OnEnable()
    {
        Enemy.OnEnemyReachedEnd += HandleEnemyReachedEnd;
        Enemy.OnEnemyDestroyed += HandleEnemyDestroyed;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyReachedEnd -= HandleEnemyReachedEnd;
        Enemy.OnEnemyDestroyed -= HandleEnemyDestroyed;

    }

    private void Start()
    {
        OnWaveChanged?.Invoke(waveCounter);
    }


    // Update is called once per frame
    void Update()
    {
        if (!isWaveActive)
        {
            return;
        }

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0 && spawnCounter < CurrentWave.enemiesPerWave)
        {
            spawnTimer = CurrentWave.spawnInterval;

            SpawnEnemy();
            spawnCounter++;
        }

        // Wave completed
        if (spawnCounter >= CurrentWave.enemiesPerWave &&
            enemiesRemoved >= CurrentWave.enemiesPerWave)
        {
            EndWave();
        }
    }

    public void StartNextWave()
    {
        if (!waitingForNextWave)
            return;

        if (currentWaveIndex >= waves.Length)
        {
            Debug.Log("All waves completed!");
            return;
        }

        Debug.Log("Starting Wave");

        isWaveActive = true;
        waitingForNextWave = false;

        spawnCounter = 0;
        enemiesRemoved = 0;
        spawnTimer = 0f;

        waveCounter++;
        OnWaveChanged?.Invoke(waveCounter);
    }

    private void EndWave()
    {
        Debug.Log("Wave Complete");

        isWaveActive = false;
        waitingForNextWave = true;

        currentWaveIndex++;
    }

    private void SpawnEnemy()
    {
        if (poolDictionary.TryGetValue(CurrentWave.enemyType, out var pool))
        {
            GameObject spawnedObject = pool.GetPooledObject();
            
            if (spawnedObject != null)
            {
                spawnedObject.transform.position = transform.position;
                spawnedObject.SetActive(true);
            }
        }
    }

    private void HandleEnemyReachedEnd(EnemyData data)
    {
        enemiesRemoved ++;
    }

    private void HandleEnemyDestroyed(Enemy enemy)
    {
        enemiesRemoved ++;
    }

    public bool CanStartWave()
    {
        return waitingForNextWave;
    }
}
