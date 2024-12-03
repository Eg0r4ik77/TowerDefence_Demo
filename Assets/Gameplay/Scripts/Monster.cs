using R3;
using UnityEngine;

namespace Gameplay.Scripts
{
	public class Monster : MonoBehaviour, ITarget
	{
		[SerializeField] private Transform _bottomPoint;
		[SerializeField] private float _speed = 10f;
		[SerializeField] private int _maxHealth = 30;
		[SerializeField] private float _reachDistance = 0.3f;
	
		public Observable<Unit> Died => _died;
		private Subject<Unit> _died = new();
	
		private Vector3 _moveTargetPosition;
		private int _currentHealth;

		public Vector3 Position
		{
			get => transform.position;
			set => transform.position = value - Vector3.up * _bottomPoint.localPosition.y;
		}

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
	
		private void OnEnable()
		{
			_currentHealth = _maxHealth;
			_died = new Subject<Unit>();
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
		
			transform.Translate (translation);
		}

		private void Die()
		{
			_died.OnNext(Unit.Default);	
		}
	}
}
