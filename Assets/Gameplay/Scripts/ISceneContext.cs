using System.Collections.Generic;

namespace Gameplay.Scripts
{
    public interface ISceneContext
    {
        public void RegisterEntity<T>(T entity) where T : ISceneEntity;
        public void UnregisterEntity<T>(T entity) where T : ISceneEntity;
        public IReadOnlyCollection<T> GetEntities<T>() where T : ISceneEntity;
    }
}