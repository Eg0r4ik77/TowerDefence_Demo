using System.Collections.Generic;
using Gameplay.System.EnemySpawn;
using Gameplay.System.Scene;
using Infrastructure;
using R3;
using UnityEngine;

namespace Gameplay.Targets.Monster
{
	public class Monster : MonoBehaviour, ITarget, IPoolObject
	{
		[SerializeField] private MonsterData _data;
		[SerializeField] private Transform _bottomPoint;
		
		[SerializeField] private float _speed;
		[SerializeField] private int _maxHealth;
		[SerializeField] private float _reachDistance;

		private readonly Subject<Unit> _died = new();
		private PointsRoute _route;
		private int _currentHealth;

		private IEnumerator<Transform> _enumerator;

		public Vector3 Position
		{
			get => transform.position;
			set => transform.position = value - Vector3.up * _bottomPoint.localPosition.y;
		}

		public Vector3 Forward => transform.forward;
		public float Speed => _speed;
		public ISceneContext SceneContext { get; set; }
		public Observable<Unit> Released => _died;
		
		public void SetRoute(PointsRoute route)
		{
			_route = route;
			
			_enumerator = route.Enumerator;
			_enumerator.MoveNext();
		}
		
		public void ApplyDamage(int damage)
		{
			_currentHealth -= damage;

			if (_currentHealth <= 0)
			{
				Die();
				return;
			}
			
			print($"DAMAGED! Health: {_currentHealth}");
		}
		
		public void Reset()
		{
			_currentHealth = _maxHealth;
		}
		
		private void Awake()
		{
			Initialize();
		}
		
		private void Update ()
		{
			if (_enumerator.Current == null)
			{
				Debug.LogError("Enumerator Current is null");
				Die();
				return;
			}
			
			if (Vector3.Distance (_bottomPoint.position, _enumerator.Current.position) <= _reachDistance)
			{
				if (!_enumerator.MoveNext())
				{
					Die();
					return;
				}

				transform.rotation = _enumerator.Current.rotation;
			}

			var direction = ( _enumerator.Current.position - _bottomPoint.position).normalized;
			var translation = direction * (_speed * Time.deltaTime);
		
			transform.Translate(translation, Space.World);
		}
		
		private void Initialize()
		{
			if(_data == null)
				return;

			_speed = _data.Speed;
			_maxHealth = _data.MaxHealth;
			_reachDistance = _data.ReachDistance;
		}

		private void Die()
		{
			SceneContext.UnregisterEntity(this);
			_died.OnNext(Unit.Default);	
		}
	}
}
