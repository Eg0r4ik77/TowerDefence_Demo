using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gameplay.Scripts
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] protected float shootTimeInterval = 0.5f;
        [SerializeField] protected float range = 4f;
        [SerializeField] protected Projectile projectilePrefab;
        [SerializeField] protected Transform shootPoint;
        
        private bool _isLoaded = true;
        private CancellationTokenSource _cancellationTokenSource;
        
        private void Update()
        {
            TryShoot();
        }

        private async void TryShoot()
        {
            if (!_isLoaded)
                return;

            if (!CheckForTarget(out ITarget target))
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
            
            foreach (var monster in FindObjectsOfType<Monster>().ToList().Cast<ITarget>())
            {
                if (Vector3.Distance(transform.position, monster.Pose.position) <= range)
                {
                    target = monster;
                    return true;
                }
            }

            return false;
        }
        

        protected virtual void Shoot(ITarget target) {}

        private void OnDestroy()
        {
            _cancellationTokenSource?.Dispose();
        }
    }
}