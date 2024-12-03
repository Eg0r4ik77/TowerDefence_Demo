namespace Gameplay.Scripts
{
	public class CannonTower : ProjectileTower<CannonProjectile>
	{
		protected override void InitializeProjectile(CannonProjectile projectile, ITarget target)
		{
			projectile.transform.position = shootPoint.position;
			projectile.transform.rotation = shootPoint.rotation;
		}
	}
}
