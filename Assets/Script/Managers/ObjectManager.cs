using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    [SerializeField]
    GameObject bulletPrefab;
    GameObject[] bullet;
    GameObject[] targetPool;
    public static ObjectManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        bullet = new GameObject[40];
        Generate();
    }

    void Generate()
    {
        for (int index = 0; index < bullet.Length; index++)
        {
            bullet[index] = Managers.Resource.Instantiate("bullet");
            bullet[index].SetActive(false);
        }
    }

    public GameObject[] GetTargetPool(string type)
    {
        switch (type)
        {
            case "bullet":
                targetPool = bullet;
                break;
        }
        return targetPool;
    }

    public GameObject MakeObj(string type)
    {
        switch (type)
        {
            case "bullet":
                targetPool = bullet;
                break;
        }

        for (int index = 0; index < targetPool.Length; index++)
        {

            if (!targetPool[index].activeSelf)
            {
                targetPool[index].SetActive(true);
                return targetPool[index];
            }
        }
        return null;
    }

    public void DelObj(string type)
    {
        switch (type)
        {
            case "bullet":
                targetPool = bullet;
                break;
        }

        for (int index = 0; index < targetPool.Length; index++)
        {
            if (Camera.main.WorldToViewportPoint(targetPool[index].transform.position).y <= -0.5f ||
                Camera.main.WorldToViewportPoint(targetPool[index].transform.position).y >= 0.5f)
            {
                targetPool[index].SetActive(false);
            }
        }
    }
}
