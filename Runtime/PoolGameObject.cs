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
            return Instantiate(Prefab, transform);
        }
        
        public void Release(T component)
        {
            // Moving very far is less costly than disabling
            // Obviously, we need to not use `Update()`, etc.
            // TODO: Profile again to makes sure
            var obj = component.gameObject;
            obj.transform.localPosition = new Vector3(-90000, -90000, -90000);
            
            //obj.SetActive(false);
            
            _pool.Enqueue(component);
            List.Remove(component);
        }
    }
}