using System;
using System.Collections.Generic;
using R3;
using UnityEngine;

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
		
		private IDisposable _spawnDisposable;
		private Dictionary<ITarget, IDisposable> _deathDisposables;
	
		private void Start()
		{
			_monsterPool = new Pool<Monster>(_monsterPrefab, transform, _maxMonsterCount);
			_deathDisposables = new Dictionary<ITarget, IDisposable>();
			_spawnDisposable = Observable.Interval(TimeSpan.FromSeconds(_interval))
				.Subscribe(_ => SpawnMonster())
				.AddTo(this);
		
			SpawnMonster();
		}

		private void OnDestroy()
		{
			_spawnDisposable?.Dispose();
			
			foreach (var disposable in _deathDisposables.Values)
				disposable.Dispose();
		}
		
		private void SpawnMonster()
		{
			var monster = SpawnEnemy(_monsterPool);
			monster.SetMoveTarget(_moveTarget.position);
		}
		
		private T SpawnEnemy<T>(Pool<T> pool) where T : Component, ITarget
		{
			ITarget enemy = pool.Get();
		
			enemy.Position = _spawnPoint.position;
			
			var disposable = enemy.Died.Subscribe(_ => pool.Release((T)enemy));
			_deathDisposables.TryAdd(enemy, disposable);
		
			return (T)enemy;
		}
	}
}
