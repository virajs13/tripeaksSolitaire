using System.Collections.Generic;
using UnityEngine;

namespace TriPeaksSolitaire.Utils
{
    public class ObjectPool<T> where T : MonoBehaviour

    {
        private readonly T prefab;
        private readonly Transform parentTransform;
        private readonly List<T> objectPool = new List<T>();

        public ObjectPool(T prefab, int initialSize)
        {
            this.prefab = prefab;
            this.parentTransform = CreateParentTransform(prefab);

            for (int i = 0; i < initialSize; i++)
            {
                T obj = CreateObject();
                obj.gameObject.SetActive(false);
                objectPool.Add(obj);
            }
        }

        public T GetObject()
        {
            if (objectPool.Count > 0)
            {
                T pooledObject = objectPool[objectPool.Count - 1];
                objectPool.RemoveAt(objectPool.Count - 1);

                // Ensure the object is active when retrieved from the pool
                pooledObject.gameObject.SetActive(true);

                return pooledObject;
            }
            else
            {
                return CreateObject();
            }
        }

        public void ReturnObject(T obj)
        {
            // Ensure the object is inactive when returned to the pool
            obj.gameObject.SetActive(false);
            objectPool.Add(obj);
        }

        private T CreateObject()
        {
            T newObj = Object.Instantiate(prefab, parentTransform);
            newObj.gameObject.name = typeof(T).Name;
            return newObj;
        }

        private Transform CreateParentTransform(T prefab)
        {
            GameObject parentObject = new GameObject(prefab.gameObject.name + "Pool");
            return parentObject.transform;
        }
    }
}