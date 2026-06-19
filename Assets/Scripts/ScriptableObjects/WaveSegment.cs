using System;
using UnityEngine;

[Serializable]
public class WaveSegment
{
    public EnemyType enemyType;

    public float spawnInterval = 1f;

    public int enemyCount = 10;
}