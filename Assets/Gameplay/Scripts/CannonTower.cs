using System;
using Gameplay.Scripts;
using R3;

public class CannonTower : Tower, IDisposable
{
	private Pool<CannonProjectile> _projectilesPool;
	private IDisposable _disposable;

	private void Start()
	{
		_projectilesPool = new Pool<CannonProjectile>(projectilePrefab as CannonProjectile, 3);
	}
	
	protected override void Shoot(ITarget target)
	{
		var projectile = _projectilesPool.Get();
		
		projectile.transform.position = shootPoint.position;
		projectile.transform.rotation = shootPoint.rotation;

		_disposable = projectile.Destroyed.Subscribe(_ => _projectilesPool.Release(projectile));
	}
	
	public void Dispose()
	{
		_disposable?.Dispose();
	}
}
