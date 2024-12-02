using UnityEngine;
using Gameplay.Scripts;

public class SimpleTower : Tower
{
	private Pool<GuidedProjectile> _projectilesPool;

	private void Start()
	{
		_projectilesPool = new(projectilePrefab as GuidedProjectile, 3);
	}

	protected override void Shoot(Monster target)
	{
		var projectile = _projectilesPool.Get();

		projectile.transform.position = transform.position + Vector3.up * 1.5f;
		projectile.m_target = target.gameObject;
		
		projectile.Destroyed += () => _projectilesPool.Release(projectile);
	}
}
