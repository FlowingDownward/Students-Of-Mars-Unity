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
    private WaveSegment CurrentSegment => CurrentWave.segments[currentSegmentIndex];

    private int currentSegmentIndex = 0;
    private int spawnedInSegment = 0;

    // Total Wave Tracking
    private int totalEnemiesThisWave = 0;
    private int enemiesRemoved = 0;

    private float spawnTimer;

    private bool isWaveActive = false;
    private bool waitingForNextWave = true;

    //Enemy Pools    
    [SerializeField] private ObjectPooler dronePool;
    [SerializeField] private ObjectPooler speederPool;
    [SerializeField] private ObjectPooler tankPool;
    [SerializeField] private ObjectPooler queenPool;
    

    private Dictionary<EnemyType, ObjectPooler> poolDictionary;

    private void Awake()
    {
        poolDictionary = new Dictionary<EnemyType,ObjectPooler>()
        {
            {EnemyType.Drone, dronePool},
            {EnemyType.Speeder, speederPool},
            {EnemyType.Tank, tankPool},
            {EnemyType.Queen, queenPool}
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

        if (spawnTimer <= 0f)
        {
            HandleSpawning();
        }

        // Entire wave complete
        if (AllEnemiesSpawned() && enemiesRemoved >= totalEnemiesThisWave)
        {
            EndWave();
        }
    }

    public void StartNextWave()
    {
        if (!waitingForNextWave)
        {
            return;
        }

        if (currentWaveIndex >= waves.Length)
        {
            Debug.Log("All waves completed!");
            return;
        }

        Debug.Log("Starting Wave");

        isWaveActive = true;
        waitingForNextWave = false;

        spawnTimer = 0f;

        currentSegmentIndex = 0;
        spawnedInSegment = 0;

        enemiesRemoved = 0;

        CalculateTotalEnemies();

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

    public bool CanStartWave()
    {
        return waitingForNextWave;
    }

    private void HandleSpawning()
    {
        // Finished all segments
        if (currentSegmentIndex >= CurrentWave.segments.Length)
            return;

        // Spawn current segment enemies
        if (spawnedInSegment < CurrentSegment.enemyCount)
        {
            SpawnEnemy(CurrentSegment.enemyType);

            spawnedInSegment++;

            spawnTimer = CurrentSegment.spawnInterval;
        }
        else
        {
            AdvanceSegment();
        }
    }

    private void AdvanceSegment()
    {
        currentSegmentIndex++;
        spawnedInSegment = 0;

        // Immediate spawn for next segment
        spawnTimer = 0f;
    }

    private bool AllEnemiesSpawned()
    {
        return currentSegmentIndex >= CurrentWave.segments.Length;
    }    

    private void SpawnEnemy(EnemyType enemyType)
    {
        if (poolDictionary.TryGetValue(enemyType, out var pool))
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

    private void CalculateTotalEnemies()
    {
        totalEnemiesThisWave = 0;

        foreach (WaveSegment segment in CurrentWave.segments)
        {
            totalEnemiesThisWave += segment.enemyCount;
        }
    }

    
}