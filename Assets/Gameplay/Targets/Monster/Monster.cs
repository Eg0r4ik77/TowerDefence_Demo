using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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
		[SerializeField] private int _currentHealth;
		[SerializeField] private float _reachDistance;

		private IEnumerator<Transform> _routeEnumerator;
		private readonly Subject<Unit> _died = new();
		
		private bool _isFlashing;

		private MeshRenderer _meshRenderer;
		private MaterialPropertyBlock _materialPropertyBlock;
		private Color _defaultBodyColor;

		public Vector3 Position
		{
			get => transform.position;
			set => transform.position = value - Vector3.up * _bottomPoint.localPosition.y;
		}

		public Vector3 Forward => transform.forward;
		public float Speed => _speed;
		public float Health => _currentHealth;
		public ISceneContext SceneContext { get; set; }
		public Observable<Unit> Released => _died;
		
		public void InitRoute(PointsRoute route)
		{
			_routeEnumerator = route.Enumerator;
			_routeEnumerator.MoveNext();
		}
		
		public void ApplyDamage(int damage)
		{
			_currentHealth -= damage;

			if (_currentHealth <= 0)
			{
				Die();
				return;
			}
			
			Flash();
		}
		
		public void Reset()
		{
			_currentHealth = _maxHealth;
			
			_isFlashing = false;
			SetColor(_defaultBodyColor);
		}
		
		private void Awake()
		{
			_meshRenderer = GetComponent<MeshRenderer>();
			
			Initialize();
		}
		
		private void Update ()
		{
			if (_routeEnumerator.Current == null)
			{
				Debug.LogError("Enumerator Current is null");
				Die();
				return;
			}
			
			if (Vector3.Distance (_bottomPoint.position, _routeEnumerator.Current.position) <= _reachDistance)
			{
				if (!_routeEnumerator.MoveNext())
				{
					Die();
					return;
				}

				transform.rotation = _routeEnumerator.Current.rotation;
			}

			var direction = ( _routeEnumerator.Current.position - _bottomPoint.position).normalized;
			var translation = direction * (_speed * Time.deltaTime);
		
			transform.Translate(translation, Space.World);
		}
		
		private void Initialize()
		{
			_defaultBodyColor = _meshRenderer.material.color;
			_materialPropertyBlock = new MaterialPropertyBlock();
			
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

		private async void Flash()
		{
			if (_isFlashing)
				return;
			
			_isFlashing = true;
			SetColor(Color.red);
			
			await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
			
			SetColor(_defaultBodyColor);
			_isFlashing = false;
		}

		private void SetColor(Color color)
		{
			_meshRenderer.GetPropertyBlock(_materialPropertyBlock);
			_materialPropertyBlock.SetColor("_Color", color);
			_meshRenderer.SetPropertyBlock(_materialPropertyBlock);
		}
	}
}
