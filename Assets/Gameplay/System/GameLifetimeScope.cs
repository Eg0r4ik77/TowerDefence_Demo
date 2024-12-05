using Gameplay.System.Scene;
using VContainer;
using VContainer.Unity;

namespace Gameplay.System
{
    public class GameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<SceneContext>(Lifetime.Singleton).As<ISceneContext>();
                
            base.Configure(builder);
        }
    }
}