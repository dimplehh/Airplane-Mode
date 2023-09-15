using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance;
    ResourceManager _resource = new ResourceManager();
    public static Managers Instance { get { init(); return s_instance; } }
    public static ResourceManager Resource { get { return Instance._resource; } }

    void Awake()
    {
        init();
    }

    static void init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { };
                go.AddComponent<Managers>();
            }
            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();
        }

    }

}