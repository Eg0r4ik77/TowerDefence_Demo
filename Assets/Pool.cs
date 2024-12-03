using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

public class Pool<T> : IObjectPool<T>, IDisposable where T : Component
{
    private int Count => objects.Count;
    private T this[int index] => objects.ElementAt(index);

    private readonly HashSet<T> objects;
    private readonly ObjectPool<T> _objectPool;

    private readonly Transform _parent;
    private readonly T _prefab;

    public Pool(T prefab, int maxCount = 10)
    {
        _prefab = prefab;

        objects = new HashSet<T>();
        _objectPool = new ObjectPool<T>(Create, OnGet, OnRelease, OnDestroy, true, 0, maxCount);
    }

    public Pool(T prefab, Transform parent, int max = 10)
    {
        _prefab = prefab;
        _parent = parent;

        objects = new HashSet<T>();
        _objectPool = new ObjectPool<T>(Create, OnGet, OnRelease, OnDestroy, true, 0, max);
    }

    public void Dispose()
    {
        _objectPool.Clear();
    }

    public T Get()
    {
        return _objectPool.Get();
    }

    public PooledObject<T> Get(out T behaviour)
    {
        return _objectPool.Get(out behaviour);
    }

    public void Release(T behaviour)
    {
        _objectPool.Release(behaviour);
    }

    public void Clear()
    {
        objects.Clear();
        _objectPool.Clear();
    }

    private void OnDestroy(T behaviour)
    {
        objects.Remove(behaviour);
        Object.Destroy(behaviour);
    }

    private void OnRelease(T behaviour)
    {
        objects.Remove(behaviour);
        behaviour.gameObject.SetActive(false);
    }

    private void OnGet(T behaviour)
    {
        objects.Add(behaviour);
        behaviour.gameObject.SetActive(true);
    }

    private T Create()
    {
        var behaviour = Object.Instantiate(_prefab, _parent);
        behaviour.gameObject.SetActive(true);

        return behaviour;
    }

    public int CountInactive => _objectPool.CountInactive;
}