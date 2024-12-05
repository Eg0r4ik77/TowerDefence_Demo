using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Gameplay.System.EnemySpawn
{
    [Serializable]
    public class EnemySpawnInstaller : IInstaller
    {
        [SerializeField] private EnemySpawnerData _data;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Transform _moveTarget;
        
        public void Install(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<EnemySpawner>()
                .WithParameter("data", _data)
                .WithParameter("spawnPoint", _spawnPoint)
                .WithParameter("moveTarget", _moveTarget);
        }
    }
}