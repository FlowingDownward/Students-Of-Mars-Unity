using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject winPanel;
    public GameObject losePanel;
    
    public static event Action<int> OnLivesChanged;
    public static event Action<int> OnCreditsChanged;

    private int playerLives = 30;
    private int playerCredits = 50;
    
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
        if (playerLives <= 0)
        {
            LoseGame();
        }
    }

    private void HandleEnemyDestroyed(Enemy enemy)
    {
        if (enemy.Data.isQueen)
        {
            WinGame();
        }
        playerCredits += enemy.Data.reward;
        OnCreditsChanged?.Invoke(playerCredits);
    }

    //Win/Loss conditions
    private void WinGame()
    {
        Time.timeScale = 0f;
        winPanel.SetActive(true);
    }

    private void LoseGame()
    {
        Time.timeScale = 0f;
        losePanel.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
    
    public void ToTitle()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }



}
