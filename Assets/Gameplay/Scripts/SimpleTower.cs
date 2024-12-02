using System;
using UnityEngine;
using Gameplay.Scripts;
using R3;

public class SimpleTower : Tower, IDisposable
{
	private Pool<GuidedProjectile> _projectilesPool;
	private IDisposable _disposable;
	
	private void Start()
	{
		_projectilesPool = new Pool<GuidedProjectile>(projectilePrefab as GuidedProjectile, 3);
	}

	protected override void Shoot(ITarget target)
	{
		var projectile = _projectilesPool.Get();

		projectile.transform.position = transform.position + Vector3.up * 1.5f;
		projectile.m_target = target;
		
		_disposable = projectile.Destroyed.Subscribe(_ => _projectilesPool.Release(projectile));
	}
	
	public void Dispose()
	{
		_disposable?.Dispose();
	}
}