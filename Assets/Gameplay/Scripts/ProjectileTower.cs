using UnityEngine;

namespace Gameplay.Scripts
{
    public abstract class ProjectileTower<TProjectile> : Tower where TProjectile : Projectile
    {
        [SerializeField] protected TProjectile _projectilePrefab;
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
            _projectilesPool = new Pool<TProjectile>(_projectilePrefab, transform, _maxProjectilesCount);
        }
    }
}