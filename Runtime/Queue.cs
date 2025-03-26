namespace JD.DS
{
    // Already alloc free
    public class Queue<T> : System.Collections.Generic.Queue<T>
    {
        public Queue() : base() { }
        public Queue(int capacity) : base(capacity) { }
    }
}