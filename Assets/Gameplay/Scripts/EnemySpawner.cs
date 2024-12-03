using System;
using R3;
using UnityEngine;
using VContainer;

namespace Gameplay.Scripts
{
	public class EnemySpawner : MonoBehaviour
	{
		[SerializeField] private float _interval = 3;

		[SerializeField] private Monster _monsterPrefab;
		[SerializeField] private int _maxMonsterCount;
	
		[SerializeField] private Transform _spawnPoint;
		[SerializeField] private Transform _moveTarget;
	
		private Pool<Monster> _monsterPool;
		private ISceneContext _sceneContext;
		private IDisposable _spawnDisposable;

		[Inject]
		private void Construct(ISceneContext sceneContext)
		{
			_sceneContext = sceneContext;
		}
		
		private void Start()
		{
			_monsterPool = new Pool<Monster>(_monsterPrefab, transform, _maxMonsterCount);
			_spawnDisposable = Observable.Interval(TimeSpan.FromSeconds(_interval))
				.Subscribe(_ => SpawnMonster())
				.AddTo(this);
		
			SpawnMonster();
		}

		private void OnDestroy()
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
