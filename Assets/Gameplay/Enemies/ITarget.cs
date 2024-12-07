using Gameplay.System.Scene;
using UnityEngine;

namespace Gameplay.Enemies
{
    public interface ITarget : ISceneEntity
    {
        public Vector3 Position { get; }
        public Vector3 Forward { get; }
        public float Speed { get; }
        public void ApplyDamage(int damage);
    }
}