using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager:MonoBehaviour
{
    string[] bulletObjs;
    public static LevelManager instance;
    bool stop;
    float[] t;
    float[] speed;
    float maxT;

    int i;
    int level;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        bulletObjs = new string[] { "bullet" };
    }
    void Start()
    {//Level Design
        t = new float[8] { 0.0f, 5.0f, 15.0f, 30.0f, 30.0f, 40.0f, 40.0f, 40.0f };
        speed = new float[8] { 2.4f, 2.7f, 3.0f, 3.3f, 3.6f, 3.9f, 4.2f, 4.5f };

        maxT = t[t.Length - 1];
        level = 0;
    }

    void Update() //stop 조건이 해지되도록 조건 추가
    {
        if (!stop)
        {
            if (t[i] > 0)
                t[i] -= Time.deltaTime;
            else
            {
                if (i < t.Length - 1)
                    i++;
                else
                {
                    t[i] = maxT;
                    speed[i] += 0.3f;
                }
                level++;
            }
        }
    }

    public void DeleteBullet()
    {
        int ranEnemy = 0;
        ObjectManager.instance.DelObj(bulletObjs[ranEnemy]);
    }

    public void SpawnBullet(Transform transform)
    {
        GameObject bullet = ObjectManager.instance.MakeObj(bulletObjs[0]);
        bullet.transform.position = transform.position;
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.up * speed[i];
    }
}
