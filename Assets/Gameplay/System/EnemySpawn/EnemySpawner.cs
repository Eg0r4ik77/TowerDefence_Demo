﻿using System;
using Gameplay.Enemies;
using Gameplay.Enemies.Monster;
using Gameplay.System.Scene;
using Infrastructure;
using R3;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Gameplay.System.EnemySpawn
{
	public class EnemySpawner : IInitializable, IStartable, IDisposable
	{
		private readonly EnemySpawnerData _data;
		private readonly Transform _spawnPoint;
		private readonly PointsRoute _route;
		private readonly ISceneContext _sceneContext;
		
		private float _interval;
		private Monster _monsterPrefab;
		private int _maxMonsterCount;
		
		private Pool<Monster> _monsterPool;
		private IDisposable _spawnDisposable;
		
		[Inject]
		public EnemySpawner(EnemySpawnerData data, Transform spawnPoint, PointsRoute route ,ISceneContext sceneContext)
		{
			_data = data;
			_spawnPoint = spawnPoint;
			_route = route;
			_sceneContext = sceneContext;
		}

		public void Initialize()
		{
			if (_data == null)
				return;

			_interval = _data.Interval;
			_monsterPrefab = _data.MonsterPrefab;
			_maxMonsterCount = _data.MaxMonsterCountInPool;
		}
		
		public void Start()
		{
			_monsterPool = new Pool<Monster>(_monsterPrefab, _spawnPoint, _maxMonsterCount);
			_spawnDisposable = Observable.Interval(TimeSpan.FromSeconds(_interval))
				.Subscribe(_ => SpawnMonster());
		
			SpawnMonster();
		}
		
		public void Dispose()
		{
			_spawnDisposable?.Dispose();
		}
		
		private void SpawnMonster()
		{
			SpawnEnemy(_monsterPool);
		}
		
		private void SpawnEnemy<T>(Pool<T> pool) where T : Enemy, IPoolObject
		{
			var enemy = pool.Get();

			enemy.Position = _spawnPoint.position;
			enemy.SetSceneContext(_sceneContext);
			enemy.transform.rotation = _spawnPoint.rotation;
			enemy.InitRoute(_route);
			
			_sceneContext.RegisterEntity(enemy);
		}
	}
}
