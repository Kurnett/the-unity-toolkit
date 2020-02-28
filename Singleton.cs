using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
  private static T _instance;
  private static object _lock = new object();
  public static T Instance {
    get {
      if (applicationIsQuitting) {
        return null;
      }

      lock (_lock) {
        if (_instance == null) {
          _instance = (T)FindObjectOfType(typeof(T));

          if (FindObjectsOfType(typeof(T)).Length > 1) {
            return _instance;
          }

          if (_instance == null) {
            GameObject singleton = new GameObject();
            _instance = singleton.AddComponent<T>();
            singleton.name = "(singleton) " + typeof(T).ToString();
            DontDestroyOnLoad(singleton);
          }
        }

        if (_instance.gameObject.scene.buildIndex != -1)
          DontDestroyOnLoad(_instance.transform.root.gameObject);

        return _instance;
      }
    }
  }

  private void Awake() {
    if (Instance != null && Instance != this)
      DestroyImmediate(gameObject);
  }

  public static bool applicationIsQuitting = false;

  public virtual void OnDestroy() {
    applicationIsQuitting = true;
  }
}