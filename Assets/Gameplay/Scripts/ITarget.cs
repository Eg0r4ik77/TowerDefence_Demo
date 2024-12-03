using R3;
using UnityEngine;

namespace Gameplay.Scripts
{
    public interface ITarget
    {
        public Vector3 Position { get; set; }
        public Observable<Unit> Died { get; }
        public void ApplyDamage(int damage);
    }
}