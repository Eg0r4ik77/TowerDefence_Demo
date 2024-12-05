using Gameplay.Projectiles;
using Gameplay.Targets;
using Infrastructure;

namespace Gameplay.Towers
{
    public abstract class ProjectileTower<TProjectile> : Tower where TProjectile : Projectile
    {
        protected TProjectile projectilePrefab;
        private int _maxProjectilesCount;

        private Pool<TProjectile> _projectilesPool;
        
        protected abstract void InitializeProjectile(TProjectile projectile, ITarget target);

        protected override void Initialize()
        {
            base.Initialize();

            var projectileTowerData = data as ProjectileTowerData;

            if (projectileTowerData == null)
                return;
            
            projectilePrefab = projectileTowerData.ProjectilePrefab as TProjectile;
            _maxProjectilesCount = projectileTowerData.MaxProjectilesCountInPool;
        }

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