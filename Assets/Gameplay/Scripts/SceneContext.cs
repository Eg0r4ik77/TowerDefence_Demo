using System.Collections.Generic;
using System.Linq;

namespace Gameplay.Scripts
{
    public class SceneContext : ISceneContext
    {
        private readonly List<ISceneEntity> _entities = new();
        
        public void RegisterEntity<T>(T entity) where T : ISceneEntity
        {
            if (_entities.Contains(entity))
                return;
            
            _entities.Add(entity);
        }

        public void UnregisterEntity<T>(T entity) where T : ISceneEntity
        {
            if (!_entities.Contains(entity))
                return;
            
            _entities.Remove(entity);
        }

        public IReadOnlyCollection<T> GetEntities<T>() where T : ISceneEntity
        {
            return _entities.OfType<T>().ToList();
        }
    }
}