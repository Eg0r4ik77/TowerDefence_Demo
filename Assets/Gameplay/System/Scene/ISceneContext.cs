using System.Collections.Generic;

namespace Gameplay.System.Scene
{
    public interface ISceneContext
    {
        public void RegisterEntity<T>(T entity) where T : ISceneEntity;
        public void UnregisterEntity<T>(T entity) where T : ISceneEntity;
        public IReadOnlyCollection<T> GetEntities<T>() where T : ISceneEntity;
    }
}