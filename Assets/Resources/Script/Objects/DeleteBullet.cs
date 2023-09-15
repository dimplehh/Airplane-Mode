using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ObjectPooler;

public class DeleteBullet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Bullet")
        {
            OP.PoolDestroy(collision.gameObject);
        }
    }
}
