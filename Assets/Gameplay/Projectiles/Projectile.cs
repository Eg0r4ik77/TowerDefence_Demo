using System;
using Gameplay.Enemies;
using Infrastructure;
using R3;
using UnityEngine;

namespace Gameplay.Projectiles
{
    public abstract class Projectile : MonoBehaviour, IPoolObject
    {
        [SerializeField] private ProjectileData _data;
        
        [SerializeField] protected float speed;
        [SerializeField] private int _damage;
        [SerializeField] private float _lifeTime;

        private readonly Subject<Unit> _destroyed = new();
        private IDisposable _lifetimeDisposable;
        
        public float Speed => _data.Speed;

        public Observable<Unit> Released => _destroyed;
        
        protected abstract void Translate();

        public void Reset()
        {
            _lifetimeDisposable = Observable
                .Timer(TimeSpan.FromSeconds(_lifeTime))
                .Subscribe(_ => Destroy());
        }

        private void Awake()
        {
            if (_data == null)
                return;
            
            speed = _data.Speed;
            _damage = _data.Damage;
            _lifeTime = _data.LifeTime;
        }

        private void Update()
        {
            Translate();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Destroy();
                return;
            }

            if (other.TryGetComponent(out ITarget target))
            {
                target.ApplyDamage(_damage);
                Destroy();   
            }
        }
        
        private void Destroy()
        {
            _lifetimeDisposable?.Dispose();
            _destroyed?.OnNext(Unit.Default);
        }
    }
}