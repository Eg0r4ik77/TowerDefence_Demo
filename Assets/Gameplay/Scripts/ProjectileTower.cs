using System;
using System.Collections.Generic;
using R3;
using UnityEngine;

namespace Gameplay.Scripts
{
    public abstract class ProjectileTower<TProjectile> : Tower, IDisposable
        where TProjectile : Projectile
    {
        [SerializeField] private TProjectile _projectilePrefab;
        [SerializeField] private int _maxProjectilesCount = 3;

        private Pool<TProjectile> _projectilesPool;
        private Dictionary<TProjectile, IDisposable> _disposables;
	
        public void Dispose()
        {
            foreach (var disposable in _disposables.Values)
                disposable.Dispose();
        }
        
        protected abstract void InitializeProjectile(TProjectile projectile, ITarget target);
        
        protected override void Shoot(ITarget target)
        {
            var projectile = _projectilesPool.Get();
            InitializeProjectile(projectile, target);
            
            var disposable = projectile.Destroyed.Subscribe(_ => _projectilesPool.Release(projectile));
            
            _disposables.TryAdd(projectile, disposable);
        }
        
        private void Start()
        {
            _projectilesPool = new Pool<TProjectile>(_projectilePrefab, transform, _maxProjectilesCount);
            _disposables = new Dictionary<TProjectile, IDisposable>();
        }
    }
}