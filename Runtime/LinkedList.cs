﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace JD.DS
{
    // A simple `LinkedList` that doesn't alloc
    // Original from https://github.com/EllanJiang/GameFramework
    public sealed class LinkedList<T> : ICollection<T>, IEnumerable<T>, ICollection, IEnumerable
    {
        private readonly System.Collections.Generic.LinkedList<T> _linkedList;
        private readonly Queue<LinkedListNode<T>> _cachedNodes;

        public LinkedList(int capacity = 0)
        {
            _linkedList = new System.Collections.Generic.LinkedList<T>();
            _cachedNodes = new Queue<LinkedListNode<T>>(capacity);
            
            for (var i = 0; i < capacity; i++)
                _cachedNodes.Enqueue(new LinkedListNode<T>(default));
        }

        public int Count => _linkedList.Count;
        public int CachedNodeCount => _cachedNodes.Count;
        
        public LinkedListNode<T> First => _linkedList.First;
        public LinkedListNode<T> Last => _linkedList.Last;
        
        public bool IsReadOnly => ((ICollection<T>) _linkedList).IsReadOnly;
        public bool IsSynchronized => ((ICollection) _linkedList).IsSynchronized;
        public object SyncRoot => ((ICollection) _linkedList).SyncRoot;
        
        public LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value)
        {
            var newNode = AcquireNode(value);
            _linkedList.AddAfter(node, newNode);
            return newNode;
        }

        public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            _linkedList.AddAfter(node, newNode);
        }

        public LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value)
        {
            var newNode = AcquireNode(value);
            _linkedList.AddBefore(node, newNode);
            return newNode;
        }

        public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            _linkedList.AddBefore(node, newNode);
        }

        public LinkedListNode<T> AddFirst(T value)
        {
            var node = AcquireNode(value);
            _linkedList.AddFirst(node);
            return node;
        }

        public void AddFirst(LinkedListNode<T> node)
        {
            _linkedList.AddFirst(node);
        }

        public LinkedListNode<T> AddLast(T value)
        {
            var node = AcquireNode(value);
            _linkedList.AddLast(node);
            return node;
        }

        public void AddLast(LinkedListNode<T> node)
        {
            _linkedList.AddLast(node);
        }

        public void Clear()
        {
            var current = _linkedList.First;
            while (current != null)
            {
                ReleaseNode(current);
                current = current.Next;
            }

            _linkedList.Clear();
        }

        public void ClearCachedNodes()
        {
            _cachedNodes.Clear();
        }

        public bool Contains(T value)
        {
            return _linkedList.Contains(value);
        }

        public void CopyTo(T[] array, int index)
        {
            _linkedList.CopyTo(array, index);
        }

        public void CopyTo(Array array, int index)
        {
            ((ICollection) _linkedList).CopyTo(array, index);
        }

        public LinkedListNode<T> Find(T value)
        {
            return _linkedList.Find(value);
        }

        public LinkedListNode<T> FindLast(T value)
        {
            return _linkedList.FindLast(value);
        }

        public bool Remove(T value)
        {
            var node = _linkedList.Find(value);
            if (node != null)
            {
                _linkedList.Remove(node);
                ReleaseNode(node);
                return true;
            }

            return false;
        }

        public void Remove(LinkedListNode<T> node)
        {
            _linkedList.Remove(node);
            ReleaseNode(node);
        }

        public void RemoveFirst()
        {
            var first = _linkedList.First;
#if UNITY_EDITOR
            if (first == null) throw new Exception("First is invalid.");
#endif      
            _linkedList.RemoveFirst();
            ReleaseNode(first);
        }

        public void RemoveLast()
        {
            var last = _linkedList.Last;
#if UNITY_EDITOR
            if (last == null) throw new Exception("Last is invalid.");
#endif
            _linkedList.RemoveLast();
            ReleaseNode(last);
        }

        private LinkedListNode<T> AcquireNode(T value)
        {
            LinkedListNode<T> node = null;
            if (_cachedNodes.Count > 0)
            {
                node = _cachedNodes.Dequeue();
                node.Value = value;
            }
            else
            {
                node = new LinkedListNode<T>(value);
            }

            return node;
        }

        private void ReleaseNode(LinkedListNode<T> node)
        {
            node.Value = default(T);
            _cachedNodes.Enqueue(node);
        }

        void ICollection<T>.Add(T value)
        {
            AddLast(value);
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(_linkedList);
        }
        
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        [StructLayout(LayoutKind.Auto)]
        public struct Enumerator : IEnumerator<T>, IEnumerator
        {
            private System.Collections.Generic.LinkedList<T>.Enumerator _enumerator;

            internal Enumerator(System.Collections.Generic.LinkedList<T> linkedList)
            {
#if UNITY_EDITOR
                if (linkedList == null) throw new Exception("Linked list is invalid.");
#endif
                
                _enumerator = linkedList.GetEnumerator();
            }

            public T Current => _enumerator.Current;

            object IEnumerator.Current => _enumerator.Current;

            public void Dispose()
            {
                _enumerator.Dispose();
            }
            
            public bool MoveNext()
            {
                return _enumerator.MoveNext();
            }
            
            void IEnumerator.Reset()
            {
                ((IEnumerator<T>) _enumerator).Reset();
            }
        }
    }
}
