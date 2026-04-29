using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private Path currentPath; 
    
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
        moveSpeed * Time.deltaTime);

        // When target reached, set new target positon
        float relativeDistance = (transform.position - _targetPosition).magnitude;
        if (relativeDistance < 0.1f)
        {
            if (_currentWaypoint < currentPath.Waypoints.Length - 1)
            {
                _currentWaypoint++;
                _targetPosition = currentPath.GetPosition(_currentWaypoint);
            }
            else
            {
                gameObject.SetActive(false);
            }
            
        }
    }
}
