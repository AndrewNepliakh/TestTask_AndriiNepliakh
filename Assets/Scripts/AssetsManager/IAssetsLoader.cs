using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public interface IAssetsLoader
{
	Task<T> InstantiateAsset<T>(Transform parent);

	Task<T> InstantiateAssetWithDI<T>(string ID, DiContainer diContainer, Transform parent = null,
		Vector3 position = new Vector3(),
		Quaternion rotation = new Quaternion());
	void UnloadAsset();
}