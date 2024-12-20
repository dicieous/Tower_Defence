using UnityEngine;
using System.Collections.Generic;

public class ObjectPooler<T> : MonoBehaviour where T : Component
{
    [System.Serializable]
    public class Pool
    {
        public T prefab;
        public int initialSize;
        public bool expandable = true;
    }

    [SerializeField] private Pool pool;
    private Queue<T> objectPool;
    private List<T> activeObjects;

    private void Awake()
    {
        objectPool = new Queue<T>();
        activeObjects = new List<T>();
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < pool.initialSize; i++)
        {
            CreateNewObject();
        }
    }

    private void CreateNewObject()
    {
        T newObject = Instantiate(pool.prefab, transform);
        newObject.gameObject.SetActive(false);
        objectPool.Enqueue(newObject);
    }

    public T GetObject()
    {
        if (objectPool.Count == 0 && !pool.expandable)
        {
            Debug.LogWarning("Pool is empty and not expandable!");
            return null;
        }

        if (objectPool.Count == 0)
        {
            CreateNewObject();
        }

        T objectToSpawn = objectPool.Dequeue();
        objectToSpawn.gameObject.SetActive(true);
        activeObjects.Add(objectToSpawn);
        
        return objectToSpawn;
    }

    public T GetObject(Vector3 position)
    {
        T obj = GetObject();
        if (obj != null)
        {
            obj.transform.position = position;
        }
        return obj;
    }

    public T GetObject(Vector3 position, Quaternion rotation)
    {
        T obj = GetObject();
        if (obj != null)
        {
            obj.transform.position = position;
            obj.transform.rotation = rotation;
        }
        return obj;
    }

    public void ReturnObject(T objectToReturn)
    {
        objectToReturn.gameObject.SetActive(false);
        activeObjects.Remove(objectToReturn);
        objectPool.Enqueue(objectToReturn);
    }

    public void ReturnAllObjects()
    {
        foreach (T obj in activeObjects.ToArray())
        {
            ReturnObject(obj);
        }
    }

    public bool IsLimitReached => activeObjects.Count == pool.initialSize;
    public int ActiveCount => activeObjects.Count;
    public int PooledCount => objectPool.Count;
}