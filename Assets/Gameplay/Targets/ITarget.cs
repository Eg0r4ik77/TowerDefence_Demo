using Gameplay.System.Scene;
using UnityEngine;

namespace Gameplay.Targets
{
    public interface ITarget : ISceneEntity
    {
        public Vector3 Position { get; set; }
        public float Speed { get; }
        public ISceneContext SceneContext { set; }
        public void ApplyDamage(int damage);
    }
}