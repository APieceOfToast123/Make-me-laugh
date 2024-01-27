using UnityEngine;

/// <summary>
/// A static instance is similar to a singleton, but instead of destroying any new
/// instances, it overrides the current instance. This is handy for resetting the state
/// and saves you doing it manually
/// </summary>
public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour {
    public static T Instance { get; private set; }
    
    //将Awake注入Instance，使其可以初始化
    protected virtual void Awake() => Instance = this as T;

    protected virtual void OnApplicationQuit() {
        Instance = null;
        Destroy(gameObject);
    }
}

/// <summary>
/// This transforms the static instance into a basic singleton. This will destroy any new
/// versions created, leaving the original instance intact
/// ============================================================
/// 单例模式，使用饿汉式（如果没有Instance 就创建，有就自我毁灭）
/// </summary>
public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour {
    protected override void Awake() {
        if (Instance != null) Destroy(gameObject);//确保只存在一个Instance对象
        base.Awake();
    }
}

/// <summary>
/// Finally we have a persistent version of the singleton. This will survive through scene
/// loads. Perfect for system classes which require stateful, persistent data. Or audio sources
/// where music plays through loading screens, etc
/// ============================================================
/// 这个抽象类的子类可以用来存储一些在切换场景后仍然需要的数据
/// </summary>
public abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour {
    protected override void Awake() {
        base.Awake();                       //base.Awake会调用父类的Awake方法，
        DontDestroyOnLoad(gameObject);      //此代码保证物体不会在切换场景的时候被删除
    }
}

