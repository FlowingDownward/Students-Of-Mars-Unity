using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private ProjectileData projectileData;
    

    protected TowerData _data;
    public ProjectileData Data => projectileData;

    private Vector3 _shootDirection;
    private float _projectileDuration;
    private Tower ownerTower;

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

            if (projectileData.isExplosiveProjectile)
            {
                Explode();
            }
            else
            {
                enemy.TakeDamage(_data.damage, ownerTower);
                Debug.Log($"Damage dealt: {_data.damage}");
            }
            
            
            if (!projectileData.isFire)
            {
                hasHit = true;
                gameObject.SetActive(false);
            }
            
        }
    }

    
    public void Shoot(TowerData data, ProjectileData projectileData, Vector3 shootDirection, Tower owner)
    {
        _data = data;
        this.projectileData = projectileData;
        _shootDirection = shootDirection;
        _projectileDuration = data.projectileDuration;
        ownerTower = owner;

        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle -90f);
    }

    private void Explode()
    {
        GameObject zone = Instantiate(projectileData.explosionZonePrefab, transform.position, Quaternion.identity);

        ExplosionZone ez = zone.GetComponent<ExplosionZone>();

        ez.Initialize(_data.damage, projectileData.explosionDuration);
    }
}
