using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData data;
    public static event Action<EnemyData> OnEnemyReachedEnd;

    private Path currentPath; 
    
    private Vector3 _targetPosition;
    private int _currentWaypoint;

    private void Awake()
    {
        currentPath = GameObject.Find("Path").GetComponent<Path>();
    }

    private void OnEnable()
    {
        _currentWaypoint = 0;
        _targetPosition = currentPath.GetPosition(_currentWaypoint);
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
            else //Reached last waypoin
            {
                OnEnemyReachedEnd?.Invoke(data);
                gameObject.SetActive(false);
            }
            
        }
    }
}
