using Gameplay.Scripts;

public class CannonTower : Tower
{
	private Pool<CannonProjectile> _projectilesPool;

	private void Start()
	{
		_projectilesPool = new(projectilePrefab as CannonProjectile, 3);
	}

	
	protected override void Shoot(Monster target)
	{
		var projectile = _projectilesPool.Get();
		
		projectile.transform.position = shootPoint.position;
		projectile.transform.rotation = shootPoint.rotation;

		projectile.Destroyed += () => _projectilesPool.Release(projectile);
	}
}
