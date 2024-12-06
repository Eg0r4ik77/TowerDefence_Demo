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
        [SerializeField] private PointsRoute _route;
        
        public void Install(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<EnemySpawner>()
                .WithParameter("data", _data)
                .WithParameter("spawnPoint", _spawnPoint)
                .WithParameter("route", _route);
        }
    }
}