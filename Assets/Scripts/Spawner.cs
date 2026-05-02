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

    private float timeBetweenWaves = 2f;
    private float waveCooldown;
    private bool isBetweenWaves = false;

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
    }

    private void OnDisable()
    {
        Enemy.OnEnemyReachedEnd -= HandleEnemyReachedEnd;
    }

    private void Start()
    {
        OnWaveChanged?.Invoke(waveCounter);
    }


    // Update is called once per frame
    void Update()
    {
        if (isBetweenWaves)
        {
            waveCooldown -= Time.deltaTime;
            if (waveCooldown <= 0f)
            {
                currentWaveIndex = (currentWaveIndex + 1) % waves.Length;
                waveCounter++;
                OnWaveChanged?.Invoke(waveCounter);
                spawnCounter = 0;
                enemiesRemoved = 0;
                spawnTimer = 0f;
                isBetweenWaves = false;

            }
        }
        else
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0 && spawnCounter < CurrentWave.enemiesPerWave)
            {
                spawnTimer = CurrentWave.spawnInterval;
                SpawnEnemy();
                spawnCounter++;
            }
            else if (spawnCounter >= CurrentWave.enemiesPerWave && enemiesRemoved >= CurrentWave.enemiesPerWave)
            {
                isBetweenWaves = true;
                waveCooldown = timeBetweenWaves;
            }
        }   
    }

    private void SpawnEnemy()
    {
        if (poolDictionary.TryGetValue(CurrentWave.enemyType, out var pool))
        {
            GameObject spawnedObject = pool.GetPooledObject();
            spawnedObject.transform.position = transform.position;
            spawnedObject.SetActive(true);
        }
    }

    private void HandleEnemyReachedEnd(EnemyData data)
    {
        enemiesRemoved ++;
    }
}
