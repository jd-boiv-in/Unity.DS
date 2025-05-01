namespace JD.DS
{
    // Already alloc free
    public class List<T> : System.Collections.Generic.List<T>
    {
        public List() : base() { }
        public List(int capacity) : base(capacity) { }
    }
}