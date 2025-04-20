using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace JD.DS
{
    public class PoolGameObject<T> : MonoBehaviour where T : MonoBehaviour
    {
        public T Prefab;
        
        [NonSerialized] public List<T> List;
        private Queue<T> _pool;

        public int Capacity = 100;
        public bool Fill = true;
        
        private void Awake()
        {
            List = new List<T>(Capacity);
            _pool = new Queue<T>(Capacity);
            if (Fill) for (var i = 0; i < Capacity; i++)
                _pool.Enqueue(Create());
            Release(Prefab);
        }

        public void Clear()
        {
            foreach (var obj in List)
            {
                Hide(obj);
                _pool.Enqueue(obj);
            }
            List.Clear();
        }
        
        public T Get()
        {
            if (!_pool.TryDequeue(out var result))
                result = Create();

            List.Add(result);
            return result;
        }
        
        public T Create()
        {
            var obj = Instantiate(Prefab, transform);
            Hide(obj);
            return obj;
        }

        private void Hide(T component)
        {
            //return;
            
            // Moving very far is less costly than disabling
            // Obviously, we need to not use `Update()`, etc.
            // TODO: Profile again to makes sure
            var obj = component.gameObject;
            obj.transform.position = new Vector3(-999999999, 0, 0);
            
            //obj.SetActive(false);
        }
        
        public void Release(T component)
        {
            Hide(component);
            _pool.Enqueue(component);
            List.Remove(component);
        }
    }
}