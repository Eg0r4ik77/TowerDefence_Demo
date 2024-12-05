using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.System.Scene;
using Gameplay.Targets;
using R3;
using UnityEngine;
using VContainer;

namespace Gameplay.Towers
{
    public abstract class Tower : MonoBehaviour
    {
        [SerializeField] protected float shootTimeInterval = 0.5f;
        [SerializeField] protected float range = 4f;
        [SerializeField] protected Transform shootPoint;
        [SerializeField] private float _checkForTargetTimeInterval = 0.3f;
        
        private ISceneContext _sceneContext;
        private CancellationTokenSource _cancellationTokenSource;
        
        private ITarget _target;
        private bool _isLoaded = true;

        protected abstract void Shoot(ITarget target);

        protected virtual bool ReadyToShoot(ITarget target) => _isLoaded;
        
        [Inject]
        private void Construct(ISceneContext sceneContext)
        {
            _sceneContext = sceneContext;
        }

        private void OnEnable()
        {
            Observable
                .Interval(TimeSpan.FromSeconds(_checkForTargetTimeInterval))
                .Subscribe(_ => CheckForTarget())
                .AddTo(this);
        }

        private void Update()
        {
            TryShoot();
        }

        private async void TryShoot()
        {
            if (_target == null)
                return;
            
            if (!ReadyToShoot(_target))
                return;
            
            Shoot(_target);
            _isLoaded = false;

            _cancellationTokenSource = new CancellationTokenSource();
            await UniTask.WaitForSeconds(shootTimeInterval, cancellationToken: _cancellationTokenSource.Token);

            _isLoaded = true;
        }
        
        private void CheckForTarget()
        {
            _target = null;
            var sceneTargets = _sceneContext.GetEntities<ITarget>();
            
            foreach (var monster in sceneTargets)
            {
                if (monster != null && Vector3.Distance(transform.position, monster.Position) <= range)
                {
                    _target = monster;
                    return;
                }
            }
        }

        private void OnDestroy()
        {
            _cancellationTokenSource?.Dispose();
        }
    }
}