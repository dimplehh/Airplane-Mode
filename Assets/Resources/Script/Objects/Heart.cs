using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ObjectPooler;

public class Heart : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("하트머금");
            OP.PoolDestroy(this.gameObject);
        }
    }
}
