using VContainer;
using VContainer.Unity;

namespace Gameplay.Scripts
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