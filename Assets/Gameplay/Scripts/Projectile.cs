using System;
using UnityEngine;

namespace Gameplay.Scripts
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] protected float speed = 0.2f;
        [SerializeField] private int _damage = 10;
        [SerializeField] private float _lifeTime = 3f;

        public Action Destroyed;
        private float _currentLifeTime;

        private void OnEnable()
        {
            _currentLifeTime = _lifeTime;
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
            if (!other.TryGetComponent(out Monster monster))
                return;

            monster.ApplyDamage(_damage);
            
            Destroy();
        }

        protected virtual void Translate(){}
        
        private void Destroy()
        {
            Destroyed?.Invoke();
            Destroyed = null;
        }
    }
}