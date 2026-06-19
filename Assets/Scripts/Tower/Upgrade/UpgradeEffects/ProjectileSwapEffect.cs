using UnityEngine;

[CreateAssetMenu(menuName = "Tower Upgrades/Projectile Swap")]
public class ProjectileSwapEffect : UpgradeEffect
{
    public Projectile newProjectile;

    public override void Apply(Tower tower)
    {
        tower.SetProjectile(newProjectile);
    }
}
