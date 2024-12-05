using System;
using R3;
using UnityEngine;

namespace Gameplay.Scripts
{
    public abstract class Projectile : MonoBehaviour, IPoolObject
    {
        [SerializeField] protected float speed = 20f;
        [SerializeField] private int _damage = 10;
        [SerializeField] private float _lifeTime = 3f;

        private readonly Subject<Unit> _destroyed = new();
        private IDisposable _lifetimeDisposable;
        
        public float Speed => speed;

        public Observable<Unit> Released => _destroyed;
        
        protected abstract void Translate();

        public void Reset()
        {
            _lifetimeDisposable = Observable
                .Timer(TimeSpan.FromSeconds(_lifeTime))
                .Subscribe(_ => Destroy());
        }
        
        private void Update()
        {
            Translate();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out ITarget target))
                return;

            target.ApplyDamage(_damage);
            Destroy();
        }
        
        private void Destroy()
        {
            _lifetimeDisposable?.Dispose();
            _destroyed?.OnNext(Unit.Default);
        }
    }
}