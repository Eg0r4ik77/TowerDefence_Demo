using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Gameplay.Scripts
{
    public abstract class Tower : MonoBehaviour
    {
        [SerializeField] protected float shootTimeInterval = 0.5f;
        [SerializeField] protected float range = 4f;
        [SerializeField] protected Transform shootPoint;
        
        private ISceneContext _sceneContext;
        private CancellationTokenSource _cancellationTokenSource;
        private bool _isLoaded = true;

        protected abstract void Shoot(ITarget target);
        
        [Inject]
        private void Construct(ISceneContext sceneContext)
        {
            _sceneContext = sceneContext;
        }
        
        private void Update()
        {
            TryShoot();
        }

        private async void TryShoot()
        {
            if (!_isLoaded || !CheckForTarget(out ITarget target))
                return;
            
            Shoot(target);
            _isLoaded = false;

            _cancellationTokenSource = new CancellationTokenSource();
            await UniTask.WaitForSeconds(shootTimeInterval, cancellationToken: _cancellationTokenSource.Token);

            _isLoaded = true;
        }
        
        private bool CheckForTarget(out ITarget target)
        {
            target = null;
            
            foreach (var monster in _sceneContext.Targets)
            {
                if (monster != null && Vector3.Distance(transform.position, monster.Position) <= range)
                {
                    target = monster;
                    return true;
                }
            }

            return false;
        }

        private void OnDestroy()
        {
            _cancellationTokenSource?.Dispose();
        }
    }
}