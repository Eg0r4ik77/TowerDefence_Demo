using System;
using Gameplay.System.Scene;
using Gameplay.Targets;
using Gameplay.Targets.Monster;
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
		private readonly Transform _moveTarget;
		private readonly ISceneContext _sceneContext;
		
		private float _interval;
		private Monster _monsterPrefab;
		private int _maxMonsterCount;
		
		private Pool<Monster> _monsterPool;
		private IDisposable _spawnDisposable;
		
		[Inject]
		public EnemySpawner(EnemySpawnerData data, Transform spawnPoint, Transform moveTarget ,ISceneContext sceneContext)
		{
			_data = data;
			_spawnPoint = spawnPoint;
			_moveTarget = moveTarget;
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
			var monster = SpawnEnemy(_monsterPool);
			monster.SetMoveTarget(_moveTarget.position);
			
			_sceneContext.RegisterEntity(monster);
		}
		
		private T SpawnEnemy<T>(Pool<T> pool) where T : Component, IPoolObject, ITarget
		{
			ITarget enemy = pool.Get();

			enemy.Position = _spawnPoint.position;
			enemy.SceneContext = _sceneContext;
			
			return (T)enemy;
		}
	}
}
