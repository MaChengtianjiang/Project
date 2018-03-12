using UnityEngine;

/// <summary>
/// Be aware this will not prevent a non singleton constructor
///   such as `T myT = new T();`
/// To prevent that, add `protected T () {}` to your singleton class.
/// 
/// As a note, this is made as MonoBehaviour because we need Coroutines.
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

	private static readonly string MANAGER_PREFAB = "Singletons/{0}";

	private static T _instance;

	public static T Instance {
		get {
			if (_instance == null) {
				_instance = CreateInstance();
			}

			return _instance;
		}
	}

	public static T CreateInstance() {
		//Object instance = Object.FindObjectOfType< T >();
		T[] componentList = Object.FindObjectsOfType<T>();

		if (componentList.Length > 1) {
			Debug.LogError("[Singleton] Something went really wrong  - there should never be more than 1 singleton! Reopenning the scene might fix it.");
			return componentList[0];
		}

		T instance = componentList.Length == 0 ? null : componentList[0];

		if (instance == null) {
			string name = typeof(T).Name;
			string path = string.Format(MANAGER_PREFAB, name);

			Object prefab = Resources.Load(path);

			// 2015.08.24 hfujii エラーチェック追加
			if (prefab == null) {
				Debug.LogError("Singletonのプレハブ " + path + " が読み込めませんでした");
			}

			GameObject singleton = Instantiate(prefab) as GameObject;
			singleton.name = name;
			DontDestroyOnLoad(singleton);
			instance = singleton.GetComponent<T>();

			Debug.Log("[Singleton] An instance of " + typeof(T) + " is needed in the scene, so '" + singleton + "' was created with DontDestroyOnLoad.");
		} else {
			Debug.Log("[Singleton] Using instance already created: " + instance.gameObject.name);
		}

		return instance;
	}

	/**
	 * When Unity quits, it destroys objects in a random order.
	 * In principle, a Singleton is only destroyed when application quits.
	 * If any script calls Instance after it have been destroyed, 
	 *   it will create a buggy ghost object that will stay on the Editor scene
	 *   even after stopping playing the Application. Really bad!
	 * So, this was made to be sure we're not creating that buggy ghost object.
	 */
	public void OnDestroy() {
		//m_applicationIsQuitting = true;
	}

	/**
	 * シングルトンクラスのオブジェクトがシーン中に存在するかどうか
	 */
	public static bool IsExists {
		get {
			return ( _instance != null );
		}
	}

	// OnDestroy只能清理object 并不清理内存
	public void Claer() {
		_instance = null;
	}
}