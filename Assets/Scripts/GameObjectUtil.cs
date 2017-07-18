using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectUtil {

	private static Dictionary<RecycleGameObject, ObjectPool> pools = new Dictionary<RecycleGameObject, ObjectPool>();

	public static GameObject Instantiate(GameObject prefab, Vector3 pos) {
		GameObject instance = null;

		var recycledScript = prefab.GetComponent<RecycleGameObject> ();

		if (recycledScript) {
			var pool = GetObjectPool (recycledScript);
			instance = pool.NextObject (pos).gameObject;
		} else {
			instance = GameObject.Instantiate (prefab);
			instance.transform.position = pos;
		}

		return instance;
	}

	public static void Destroy(GameObject gameObject) {
		var recycleGameObject = gameObject.GetComponent<RecycleGameObject> ();

		// Test to see if the GameObject uses RecycleGameObject. If so, then
		// don't destroy it!!!
		if (recycleGameObject != null) {
			recycleGameObject.Shutdown ();
		} else {
			GameObject.Destroy (gameObject);
		}
	}

	private static ObjectPool GetObjectPool(RecycleGameObject type) {
		ObjectPool pool = null;

		if (pools.ContainsKey (type)) {
			pool = pools [type];
		} else {
			var poolContainer = new GameObject (type.gameObject.name + "ObjectPool");

			pool = poolContainer.AddComponent<ObjectPool> ();
			pool.prefab = type;
			pools.Add (type, pool);
		}

		return pool;
	}
}
