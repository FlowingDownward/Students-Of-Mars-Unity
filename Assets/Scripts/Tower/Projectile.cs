using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected TowerData _data;
    private Vector3 _shootDirection;
    private float _projectileDuration;
    [SerializeField] private bool isFire;

    void Start()
    {
        transform.localScale = Vector3.one * _data.projectileSize;
    }

    void Update()
    {
        if (_projectileDuration <= 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            _projectileDuration -= Time.deltaTime;
            transform.position += new Vector3(_shootDirection.x, _shootDirection.y) * _data.projectileSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            
            enemy.TakeDamage(_data.damage);

            if (enemy != null)
            {
                OnHit(enemy);
            }

            if (!isFire)
            {
                gameObject.SetActive(false);
            }
        }
    }

    //To override
    protected virtual void OnHit(Enemy enemy)
    {
        enemy.TakeDamage(_data.damage);
    }


    public void Shoot(TowerData data, Vector3 shootDirection)
    {
        _data = data;
        _shootDirection = shootDirection;
        _projectileDuration = data.projectileDuration;

        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle -90f);
    }
}
