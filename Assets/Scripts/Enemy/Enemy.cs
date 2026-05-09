using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData data;
    public EnemyData Data => data; //For reward handling
    public static event Action<EnemyData> OnEnemyReachedEnd;
    public static event Action<Enemy> OnEnemyDestroyed;

    private Path currentPath; 
    
    private Vector3 _targetPosition;
    private int _currentWaypoint;
    private float _health;

    private bool isDead = false;

    [SerializeField] private Transform healthBar;
    private Vector3 healthBarOriginalScale;

    private void Awake()
    {
        currentPath = GameObject.Find("Path").GetComponent<Path>();
        healthBarOriginalScale = healthBar.localScale;
    }

    private void OnEnable()
    {
        _currentWaypoint = 0;
        _targetPosition = currentPath.GetPosition(_currentWaypoint);
        _health = data.health;
        isDead = false;
        UpdateHealthBar();
    }

    void Update()
    {
        // Move towards target destination
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition,
        data.speed * Time.deltaTime);

        // When target reached, set new target positon
        float relativeDistance = (transform.position - _targetPosition).magnitude;
        if (relativeDistance < 0.1f)
        {
            if (_currentWaypoint < currentPath.Waypoints.Length - 1)
            {
                _currentWaypoint++;
                _targetPosition = currentPath.GetPosition(_currentWaypoint);
            }
            else //Reached last waypoint
            {
                OnEnemyReachedEnd?.Invoke(data);
                gameObject.SetActive(false);
            }
            
        }
    }

    public void TakeDamage (float damage)
    {
        if (isDead) return;

        _health -= damage;
        Debug.Log($"Health Left: {_health}");
        _health = Math.Max(_health, 0);
        UpdateHealthBar();

        if (_health <= 0)
        {
            if (isDead) return;

            isDead = true;
            
            OnEnemyDestroyed?.Invoke(this);
            gameObject.SetActive(false);
        }
    }

    private void UpdateHealthBar()
    {
        float healthPercent = _health / data.health;
        Vector3 scale = healthBarOriginalScale;
        scale.x = healthBarOriginalScale.x * healthPercent;
        healthBar.localScale = scale;
    }

}
