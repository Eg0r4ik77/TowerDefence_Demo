using UnityEngine;

namespace Gameplay.Scripts
{
    public interface ITarget
    {
        public Pose Pose { get; }
        public void ApplyDamage(int damage);
    }
}