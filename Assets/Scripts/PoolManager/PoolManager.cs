using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;


public class PoolManager 
{
    private static Dictionary<Type, Pool> _pools = new Dictionary<Type, Pool>();
    private GameObject _poolsGO;
    
    [Inject] private DiContainer _diContainer;

    public void Release<T>(T poolable) where T : MonoBehaviour, IPoolable
    {
        GetPool<T>().Deactivate(poolable);
    }
    
    public async Task<T> GetOrCreate<T>(Transform parent, Vector3 localPosition = default,
        Quaternion localRotation = default, int poolSize = 0) where T : MonoBehaviour, IPoolable
    {
        AddPool(typeof(T), poolSize);
        Pool pool = GetPool<T>();
        var poolCount = pool.GetCount();
        if (poolCount == 0) return await pool.Spawn<T>(parent, _diContainer, localPosition, localRotation);
        return GetPool<T>().Activate<T>(localPosition, localRotation);
    }
    
    private void AddPool(Type type, int poolSize = 0)
    {
        if (_poolsGO == null) _poolsGO = GameObject.Find("[POOLS]") ?? new GameObject("[POOLS]");

        if (!_pools.ContainsKey(type))
        {
            var poolGO = new GameObject("Pool: " + type.ToString().ToUpper());
            poolGO.transform.SetParent(_poolsGO.transform);
            var pool = poolGO.AddComponent<Pool>();
            pool.InitPool(poolGO.transform, poolSize);
            _pools.Add(type, pool);
        }
    }

    private Pool GetPool<T>() where T : MonoBehaviour, IPoolable
    {
        if (_pools.TryGetValue(typeof(T), out var pool))
        {
            return pool;
        }

        return null;
    }

    private void ClearPools()
    {
        _pools.Clear();
    }
}