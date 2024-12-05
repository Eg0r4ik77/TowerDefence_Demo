using Gameplay.System.Scene;
using Infrastructure;
using R3;
using UnityEngine;

namespace Gameplay.Targets.Monster
{
	public class Monster : MonoBehaviour, ITarget, IPoolObject
	{
		[SerializeField] private Transform _bottomPoint;
		[SerializeField] private float _speed = 10f;
		[SerializeField] private int _maxHealth = 30;
		[SerializeField] private float _reachDistance = 0.3f;

		private readonly Subject<Unit> _died = new();
		private Vector3 _moveTargetPosition;
		private int _currentHealth;

		public Vector3 Position
		{
			get => transform.position;
			set => transform.position = value - Vector3.up * _bottomPoint.localPosition.y;
		}
		public float Speed => _speed;
		public ISceneContext SceneContext { get; set; }
		public Observable<Unit> Released => _died;
		

		public void SetMoveTarget(Vector3 moveTargetPosition)
		{
			_moveTargetPosition = moveTargetPosition;
		}
		
		public void ApplyDamage(int damage)
		{
			_currentHealth -= damage;
			if(_currentHealth <= 0)
				Die();
		}
		
		public void Reset()
		{
			_currentHealth = _maxHealth;
		}

		private void Update () 
		{
			if (Vector3.Distance (_bottomPoint.position, _moveTargetPosition) <= _reachDistance) 
			{
				Die();
				return;
			}

			var direction = (_moveTargetPosition - _bottomPoint.position).normalized;
			var translation = direction * (_speed * Time.deltaTime);
		
			transform.Translate(translation, Space.World);
		}

		private void Die()
		{
			SceneContext.UnregisterEntity(this);
			_died.OnNext(Unit.Default);	
		}
	}
}
