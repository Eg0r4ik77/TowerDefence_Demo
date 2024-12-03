using System.Collections.Generic;

namespace Gameplay.Scripts
{
    public interface ISceneContext
    {
        public List<ITarget> Targets { get; }
    }
    
    public class SceneContext : ISceneContext
    {
        public List<ITarget> Targets { get; } = new();
    }
}