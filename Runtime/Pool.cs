namespace JD.DS
{
    public interface IPoolable<T>
    {
        //public T Get();
        public T Create();
        //public void Release(T bag);
    }
    
    public class Pool<T> where T : class
    {
        private readonly IPoolable<T> _poolable;
        private readonly Queue<T> _pool;

        public Pool(IPoolable<T> poolable, int capacity = 100, bool fill = true)
        {
            _poolable = poolable;
            _pool = new Queue<T>(capacity);
            if (fill) for (var i = 0; i < capacity; i++)
                _pool.Enqueue(poolable.Create());
        }
        
        public T Get()
        {
            if (_pool.TryDequeue(out var result))
                return result;
            
            return _poolable?.Create();
        }

        public void Release(T item)
        {
            _pool.Enqueue(item);
        }
    }
}