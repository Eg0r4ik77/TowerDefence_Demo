using System;
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
        private CompositeDisposable _compositeDisposable;
	
        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }
        
        protected abstract void InitializeProjectile(TProjectile projectile, ITarget target);
        
        protected override void Shoot(ITarget target)
        {
            var projectile = _projectilesPool.Get();
            InitializeProjectile(projectile, target);
            
            var disposable = projectile.Destroyed.Subscribe(_ => _projectilesPool.Release(projectile));
            _compositeDisposable.Add(disposable);
        }
        
        private void Start()
        {
            _projectilesPool = new Pool<TProjectile>(_projectilePrefab, _maxProjectilesCount);
            _compositeDisposable = new CompositeDisposable();
        }
    }
}