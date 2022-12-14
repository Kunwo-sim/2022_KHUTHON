using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static readonly object Syncobj = new object();
    private static bool _appIsClosing;

    public static T Instance
    {
        get
        {
            if (_appIsClosing)
                return null;

            lock (Syncobj)  
            {
                if (_instance != null) 
                    return _instance;
                
                var objs = FindObjectsOfType<T>();

                if (objs.Length > 0)
                    _instance = objs[0];

                if (objs.Length > 1)
                    Debug.LogError("There is more than one " + typeof(T).Name + " in the scene.");

                if (_instance != null) 
                    return _instance;
                
                var goName = typeof(T).ToString();
                var go = GameObject.Find(goName);
                
                if (go == null)
                    go = new GameObject(goName);
                
                _instance = go.AddComponent<T>();
                
                return _instance;
            }
        }
    }

    /// <summary>
    /// When Unity quits, it destroys objects in a random order.
    /// In principle, a Singleton is only destroyed when application quits.
    /// If any script calls Instance after it have been destroyed,
    ///   it will create a buggy ghost object that will stay on the Editor scene
    ///   even after stopping playing the Application. Really bad!
    /// So, this was made to be sure we're not creating that buggy ghost object.
    /// ---------------------------------------------------------------------------
    /// Unity가 종료되면, Unity는 임의의 순서로 객체를 파괴합니다.
    /// 원칙적으로 싱글톤은 응용 프로그램이 종료될 때만 파괴됩니다.
    /// 인스턴스가 파괴 된 후에 스크립트가 인스턴스를 호출하면 응용 프로그램 재생을
    /// 중지 한 후에도 편집기 장면에 머물러 있는 버그인 유령 객체가 생성됩니다.
    /// 그래서 아래 코드는 우리가 유령 객체를 만들지 않도록 하기 위한 것입니다.
    /// </summary>
    protected virtual void OnApplicationQuit()
    {
        // 실행 해제 시 참조를 해제합니다.
        _appIsClosing = true;
    }
}
