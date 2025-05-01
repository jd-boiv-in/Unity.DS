namespace JD.DS
{
    // Already alloc free
    public class Dictionary<T1, T2> : System.Collections.Generic.Dictionary<T1, T2>
    {
        public Dictionary() : base() { }
        public Dictionary(int capacity) : base(capacity) { }
    }
}