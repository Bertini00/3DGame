using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField]
    private IdContainer _ID;
    [SerializeField]
    private PoolableObject _PoolableObjectPrefab;
    [SerializeField]
    private int _PoolQuantity;
    [SerializeField]
    private int _ObjectPerFrame;

    public string ID => _ID.Id;
    private Queue<PoolableObject> _poolQueue;

    public void Setup()
    {
        StartCoroutine(AsyncInstantiate());
    }

    private IEnumerator AsyncInstantiate()
    {
        _poolQueue = new Queue<PoolableObject>();

        PoolableObject poolableObject;
        for (int i = 1; i <= _PoolQuantity; ++i)
        {
            poolableObject = Instantiate(_PoolableObjectPrefab, gameObject.transform);
            poolableObject.gameObject.SetActive(false);
            _poolQueue.Enqueue(poolableObject);
            if ((i % _ObjectPerFrame) == 0)
                yield return null;
        }
    }

    public T GetPoolableObject<T>() where T : PoolableObject
    {
        if (_poolQueue.Count == 0)
        {
            Debug.LogError("Queue ended at: " + gameObject.name);
            return null;
        }
        T obj = _poolQueue.Dequeue() as T;
        obj.gameObject.SetActive(true);
        obj.Setup();
        return obj;
    }

    public void ReturnPoolableObject(PoolableObject poolableObject)
    {
        poolableObject.Clear();
        poolableObject.gameObject.SetActive(false);
        _poolQueue.Enqueue(poolableObject);
    }
}
