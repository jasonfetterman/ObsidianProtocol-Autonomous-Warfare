using UnityEngine;
using System.Collections.Generic;

public class ObjectPool
{
    private readonly Dictionary<string, Queue<GameObject>> pools = new();

    public GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        string key = prefab.name;

        if (!pools.TryGetValue(key, out var queue))
        {
            queue = new Queue<GameObject>();
            pools[key] = queue;
        }

        if (queue.Count > 0)
        {
            GameObject obj = queue.Dequeue();
            obj.transform.SetPositionAndRotation(pos, rot);
            obj.SetActive(true);
            return obj;
        }

        GameObject newObj = Object.Instantiate(prefab, pos, rot);
        newObj.name = key;
        return newObj;
    }

    public void Despawn(GameObject obj)
    {
        obj.SetActive(false);

        string key = obj.name;

        if (!pools.TryGetValue(key, out var queue))
        {
            queue = new Queue<GameObject>();
            pools[key] = queue;
        }

        queue.Enqueue(obj);
    }
}
