using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    Dictionary<string, Queue<GameObject>> pools = new();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        string key = prefab.name;

        if (!pools.ContainsKey(key))
            pools[key] = new Queue<GameObject>();

        if (pools[key].Count > 0)
        {
            GameObject obj = pools[key].Dequeue();
            obj.transform.position = pos;
            obj.transform.rotation = rot;
            obj.SetActive(true);
            return obj;
        }

        GameObject newObj = Instantiate(prefab, pos, rot);
        newObj.name = key;
        return newObj;
    }

    public void Despawn(GameObject obj)
    {
        obj.SetActive(false);

        string key = obj.name;
        if (!pools.ContainsKey(key))
            pools[key] = new Queue<GameObject>();

        pools[key].Enqueue(obj);
    }
}
