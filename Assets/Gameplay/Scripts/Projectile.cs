using System;
using R3;
using UnityEngine;

namespace Gameplay.Scripts
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] protected float speed = 0.2f;
        [SerializeField] private int _damage = 10;
        [SerializeField] private float _lifeTime = 3f;

        public Observable<Unit> Destroyed => _destroyed;
        private Subject<Unit> _destroyed = new();
        
        private float _currentLifeTime;

        private void OnEnable()
        {
            _currentLifeTime = _lifeTime;
            _destroyed = new Subject<Unit>();
        } 

        private void Update()
        {
            _currentLifeTime -= Time.deltaTime;
            
            if (_currentLifeTime <= 0)
            {
                Destroy();
                return;
            }
            
            Translate();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out ITarget target))
                return;

            target.ApplyDamage(_damage);
            
            Destroy();
        }

        protected virtual void Translate(){}
        
        private void Destroy()
        {
            _destroyed.OnNext(Unit.Default);
        }
    }
}