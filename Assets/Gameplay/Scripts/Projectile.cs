using System;
using R3;
using UnityEngine;

namespace Gameplay.Scripts
{
    public abstract class Projectile : MonoBehaviour
    {
        [SerializeField] protected float speed = 0.2f;
        [SerializeField] private int _damage = 10;
        [SerializeField] private float _lifeTime = 3f;

        public Observable<Unit> Destroyed => _destroyed;
        private Subject<Unit> _destroyed = new();
        
        private IDisposable _disposable;

        protected abstract void Translate();
        
        private void OnEnable()
        {
            _destroyed = new Subject<Unit>();
            _disposable = Observable
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
            _destroyed?.OnNext(Unit.Default);
            _disposable?.Dispose();
        }
    }
}