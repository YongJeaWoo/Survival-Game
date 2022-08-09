using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectPooling
{
    Queue<GameObject> poolingObj = new Queue<GameObject>();

    public UnityAction OnRePooling;

    public void Push(GameObject obj)
    {
        obj.SetActive(false);
        poolingObj.Enqueue(obj);
    }

    public GameObject Pop(Vector3 pos, Quaternion rotate)
    {
        Debug.Log(poolingObj.Count);
        if (0 == poolingObj.Count)
        {
            Debug.Log("2");
            OnRePooling?.Invoke();

            Debug.Log("3");
        }
       

        GameObject obj = poolingObj.Dequeue();

        if (Vector3.zero == pos)
            pos = obj.transform.position;

        obj.transform.position = pos;
        obj.transform.rotation = rotate;
        return obj;
    }

    public int GetPoolCount()
    {
        return poolingObj.Count;
    }
}
