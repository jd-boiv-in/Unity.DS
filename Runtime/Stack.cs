namespace JD.DS
{
    // Already alloc free
    public class Stack<T> : System.Collections.Generic.Stack<T>
    {
        public Stack() : base() { }
        public Stack(int capacity) : base(capacity) { }
    }
}