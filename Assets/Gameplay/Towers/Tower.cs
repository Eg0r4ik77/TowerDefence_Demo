using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.Enemies;
using Gameplay.System.Scene;
using R3;
using UnityEngine;
using VContainer;

namespace Gameplay.Towers
{
    public abstract class Tower : MonoBehaviour
    {
        [SerializeField] protected TowerData data;
        [SerializeField] protected Transform shootPoint;
        
        [SerializeField] protected float shootTimeInterval;
        [SerializeField] protected float range;
        [SerializeField] private float _checkForTargetTimeInterval;
        
        protected ITarget shootTarget;
        
        private ISceneContext _sceneContext;
        private CancellationTokenSource _cancellationTokenSource;
        
        private bool _isLoaded = true;

        protected abstract void Shoot(ITarget target);

        protected virtual bool ReadyToShoot(ITarget target) => _isLoaded;

        protected virtual void Initialize()
        {
            if (data == null)
                return;
            
            shootTimeInterval = data.ShootTimeInterval;
            range = data.Range;
            _checkForTargetTimeInterval = data.CheckForTargetTimeInterval;
        }
        
        [Inject]
        private void Construct(ISceneContext sceneContext)
        {
            _sceneContext = sceneContext;
        }

        private void Awake()
        {
            Initialize();
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
            if (shootTarget == null)
                return;
            
            if (!ReadyToShoot(shootTarget))
                return;
            
            Shoot(shootTarget);
            _isLoaded = false;

            _cancellationTokenSource = new CancellationTokenSource();
            await UniTask.WaitForSeconds(shootTimeInterval, cancellationToken: _cancellationTokenSource.Token);

            _isLoaded = true;
        }
        
        private void CheckForTarget()
        {
            shootTarget = null;
            var sceneTargets = _sceneContext.GetEntities<ITarget>();
            
            foreach (var monster in sceneTargets)
            {
                if (monster != null && Vector3.Distance(transform.position, monster.Position) <= range)
                {
                    shootTarget = monster;
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