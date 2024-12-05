using Gameplay.System.EnemySpawn;
using Gameplay.System.Scene;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Gameplay.System
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private EnemySpawnInstaller _enemySpawnInstaller;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<SceneContext>(Lifetime.Singleton).As<ISceneContext>();
                
            _enemySpawnInstaller.Install(builder);
        }
    }
}