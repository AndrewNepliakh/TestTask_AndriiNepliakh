using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class Pool : MonoBehaviour
{
    private Queue<IPoolable> _pool;

    public void InitPool(Transform parent, int size = 0)
    {
        transform.SetParent(parent);
        _pool = new Queue<IPoolable>(size);
    }

    public async Task<T> Spawn<T>(Transform parent,DiContainer diContainer, Vector3 localPosition = default,
        Quaternion localRotation = default) where T : MonoBehaviour, IPoolable
    {
        T go;
        
        var loader = new AssetsLoader();
        

        if (localPosition == default && localRotation == default)
        {
            go = await loader.InstantiateAssetWithDI<T>(typeof(T).ToString(), diContainer);
        }
        else
        {
            go = await loader.InstantiateAssetWithDI<T>(typeof(T).ToString(), diContainer, parent, localPosition, localRotation);
        }

        return go;
    }

    public T Activate<T>(Vector3 localPosition = default, Quaternion localRotation = default)
        where T : MonoBehaviour, IPoolable
    {
        var poolable = (T) _pool.Dequeue();

        poolable.transform.localPosition = localPosition;
        poolable.transform.localRotation = localRotation;
        poolable.OnActivate();

        return poolable;
    }

    public void Deactivate<T>(T obj, object args = default) where T : MonoBehaviour, IPoolable
    {
        _pool.Enqueue(obj);
        obj.OnDeactivate(args);
    }

    public int GetCount()
    {
        return _pool.Count;
    }
}
