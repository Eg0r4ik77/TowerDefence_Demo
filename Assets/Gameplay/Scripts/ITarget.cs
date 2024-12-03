using UnityEngine;

namespace Gameplay.Scripts
{
    public interface ITarget : ISceneEntity
    {
        public Vector3 Position { get; set; }
        public ISceneContext SceneContext { set; }
        public void ApplyDamage(int damage);
    }
}