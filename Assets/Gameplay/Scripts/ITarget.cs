using R3;
using UnityEngine;

namespace Gameplay.Scripts
{
    public interface ITarget
    {
        public Vector3 Position { get; set; }
        public void ApplyDamage(int damage);
    }
}