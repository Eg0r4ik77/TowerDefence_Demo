using System;
using System.Collections.Generic;
using Gameplay;
using R3;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

public class Pool<T> : IObjectPool<T>, IDisposable where T : Component, IPoolObject
{
    private readonly ObjectPool<T> _objectPool;
    private readonly Transform _parent;
    private readonly T _prefab;
    private readonly Dictionary<T, IDisposable> _releaseDisposables = new();
    
    public int CountInactive => _objectPool.CountInactive;
    
    public Pool(T prefab, Transform parent, int max = 10) : this(prefab, max)
    {
        _parent = parent;
    }

    private Pool(T prefab, int maxCount = 10)
    {
        _prefab = prefab;
        _objectPool = new ObjectPool<T>(Create, OnGet, OnRelease, OnDestroy, true, 0, maxCount);
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
        _objectPool.Clear();
        ClearDisposables();
    }
    
    public void Dispose()
    {
        Clear();
    }

    private T Create()
    {
        var behaviour = Object.Instantiate(_prefab, _parent);
        
        behaviour.gameObject.SetActive(true);
        
        var disposable = behaviour.Released.Subscribe(_ => Release(behaviour));
        _releaseDisposables.TryAdd(behaviour, disposable);
        
        return behaviour;
    }
    
    private void OnGet(T behaviour)
    {
        behaviour.Reset();
        behaviour.gameObject.SetActive(true);
    }

    private void OnRelease(T behaviour)
    {
        behaviour.gameObject.SetActive(false);
    }
    
    private void OnDestroy(T behaviour)
    {
        if (_releaseDisposables.TryGetValue(behaviour, out var disposable))
        {
            disposable.Dispose();
            _releaseDisposables.Remove(behaviour);
        }
        
        Object.Destroy(behaviour);
    }

    private void ClearDisposables()
    {
        foreach (var disposable in _releaseDisposables.Values)
            disposable.Dispose();
        
        _releaseDisposables.Clear();
    }
}
