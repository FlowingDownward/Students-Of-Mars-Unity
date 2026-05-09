using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected TowerData _data;
    private Vector3 _shootDirection;
    private float _projectileDuration;
    [SerializeField] private bool isFire;

    private bool hasHit;

    private void OnEnable()
    {
        hasHit = false;
    }

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
        if (hasHit) return;

        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            if (_data.isExplosiveProjectile)
            {
                OnHit(enemy);
            }
            else
            {
                enemy.TakeDamage(_data.damage);
                Debug.Log($"Damage dealt: {_data.damage}");
            }
            

            if (!isFire)
            {
                hasHit = true;
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
