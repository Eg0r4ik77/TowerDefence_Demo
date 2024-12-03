using UnityEngine;

namespace Gameplay.Scripts
{
    public interface ITarget
    {
        public Vector3 Position { get; }
        public void ApplyDamage(int damage);
    }
}