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
            Debug.Log("��Ʈ�ӱ�");
            OP.PoolDestroy(this.gameObject);
        }
    }
}
