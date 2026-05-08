using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action<int> OnLivesChanged;
    public static event Action<int> OnCreditsChanged;

    private int playerLives = 30;
    private int playerCredits = 20;
    
    public int PlayerCredits => playerCredits;

    public static GameManager Instance;

    private void Start()
    {
        OnLivesChanged?.Invoke(playerLives);
        OnCreditsChanged?.Invoke(playerCredits);
    }

    private void Awake()
    {
        Instance = this;
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

    public bool TrySpendCredits(int amount)
    {
        if (playerCredits < amount)
            return false;

        playerCredits -= amount;
        OnCreditsChanged?.Invoke(playerCredits);
        return true;
    }

    private void HandleEnemyReachedEnd(EnemyData data){
        
        playerLives = Mathf.Max(0, playerLives - data.damage);
        OnLivesChanged?.Invoke(playerLives);
    }

    private void HandleEnemyDestroyed(Enemy enemy)
    {
        playerCredits += enemy.Data.reward;
        OnCreditsChanged?.Invoke(playerCredits);
    }

}
