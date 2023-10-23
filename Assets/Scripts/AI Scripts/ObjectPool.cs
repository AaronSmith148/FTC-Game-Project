using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private PoolableObject Prefab;
    private List<PoolableObject> AvailableObjects;

    private ObjectPool(PoolableObject Prefab, int Size)
    {
        this.Prefab = Prefab;
        AvailableObjects = new List<PoolableObject>(Size);
    }

    //Make the list of objects
    public static ObjectPool CreateInstance(PoolableObject Prefab, int Size)
    {
        ObjectPool pool = new ObjectPool(Prefab, Size);

        GameObject poolObject = new GameObject(Prefab.name + " Pool");
        pool.CreateObjects(poolObject.transform, Size);

        return pool;
    }

    //Create/Instantiate the objects
    private void CreateObjects(Transform parent, int Size)
    {
        for(int i = 0; i < Size; i++)
        {
            PoolableObject poolableObject = GameObject.Instantiate(Prefab, Vector3.zero, Quaternion.identity, parent.transform);
            poolableObject.Parent = this;
            poolableObject.gameObject.SetActive(false);
        }
    }

    // I don't think we need this but better to have it just in case
    // Way to return object to pool
    public void ReturnObjectToPool(PoolableObject poolableObject)
    {
        AvailableObjects.Add(poolableObject);
    }

    //Choose the object to instantiate and iterate through list
    public PoolableObject GetObject()
    {
        if(AvailableObjects.Count > 0)
        {
            PoolableObject instance = AvailableObjects[0];
            AvailableObjects.RemoveAt(0);
            instance.gameObject.SetActive(true);

            return instance;
        }
        else
        {
            Debug.LogError($"Could not get an AI from pool \"{Prefab.name}\" Pool.");
            return null;
        }
    }
}
