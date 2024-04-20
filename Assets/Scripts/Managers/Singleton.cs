using NaughtyAttributes;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    [Header("Singleton Settings")]
    [SerializeField] private bool dontDestroyOnLoad = true;
    [Tooltip("If true the whole duplicate obj will be destroyed, if false just the duplicate script is destroyed")]
    [SerializeField] private bool destroyObjToEnsureInstance = true;

    #region Properties
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance != null) return instance;

            // Search for existing instance.
            instance = (T)FindObjectOfType(typeof(T));

            return instance;
        }
    }
    public static bool InstanceIsValid => Instance != null;
    #endregion

    #region Unity Messages
    protected virtual void Awake()
    {
        if (Instance != null && Instance != this)
        {
            if (destroyObjToEnsureInstance)
                Destroy(gameObject);
            else
                Destroy(this);
        }

        if (dontDestroyOnLoad)
            DontDestroyOnLoad(gameObject);
    }
    #endregion
}