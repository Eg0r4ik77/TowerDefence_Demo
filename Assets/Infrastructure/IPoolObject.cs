using R3;

namespace Infrastructure
{
    public interface IPoolObject
    {
        public void Reset();
        public Observable<Unit> Released { get; }
    }
}