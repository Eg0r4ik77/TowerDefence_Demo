using R3;

namespace Gameplay
{
    public interface IPoolObject
    {
        public void Reset();
        public Observable<Unit> Released { get; }
    }
}