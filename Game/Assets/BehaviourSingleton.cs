using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourSingleton<T> : MonoBehaviour where T : BehaviourSingleton<T>
{
    public static T obj
    {
        get;
        private set;
    }

    public virtual void Awake()
    {
        if (obj != null)
            Debug.LogError("Assigning a singleton twice: " + typeof(T).Name);

        obj = this.GetComponent<T>();
    }

    public virtual void OnDestroy()
    {
        if (this == obj)
            obj = null;
    }
}
