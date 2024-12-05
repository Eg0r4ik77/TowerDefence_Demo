using Gameplay.Projectiles;
using Gameplay.Targets;
using Infrastructure;
using UnityEngine;

namespace Gameplay.Towers
{
    public abstract class ProjectileTower<TProjectile> : Tower where TProjectile : Projectile
    {
        [SerializeField] protected TProjectile projectilePrefab;
        [SerializeField] private int _maxProjectilesCount = 3;

        private Pool<TProjectile> _projectilesPool;
        
        protected abstract void InitializeProjectile(TProjectile projectile, ITarget target);
        
        protected override void Shoot(ITarget target)
        {
            var projectile = _projectilesPool.Get();
            InitializeProjectile(projectile, target);
        }
        
        private void Start()
        {
            _projectilesPool = new Pool<TProjectile>(projectilePrefab, transform, _maxProjectilesCount);
        }
    }
}