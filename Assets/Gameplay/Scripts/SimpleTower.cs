using UnityEngine;

namespace Gameplay.Scripts
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