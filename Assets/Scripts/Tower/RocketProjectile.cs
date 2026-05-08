using UnityEngine;

public class RocketProjectile : Projectile
{
    [SerializeField] private GameObject explosionZonePrefab;

    protected override void OnHit(Enemy enemy)
    {
        Explode();
    }

    private void Explode()
    {
        GameObject zone = Instantiate(
            explosionZonePrefab,
            transform.position,
            Quaternion.identity
        );

        ExplosionZone ez = zone.GetComponent<ExplosionZone>();

        ez.Initialize(
            _data.damage,
            _data.explosionDuration
        );
    }
}