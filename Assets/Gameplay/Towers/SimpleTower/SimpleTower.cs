using Gameplay.Enemies;
using Gameplay.Projectiles.GuidedProjectile;
using UnityEngine;

namespace Gameplay.Towers.SimpleTower
{
	public class SimpleTower : ProjectileTower<GuidedProjectile>
	{
		protected override void InitializeProjectile(GuidedProjectile projectile, ITarget target)
		{
			projectile.SetTarget(target);
			projectile.transform.position = transform.position + Vector3.up * 1.5f;
		}
	}
}