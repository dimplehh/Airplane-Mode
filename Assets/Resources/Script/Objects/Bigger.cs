using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ObjectPooler;

public class Bigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("���ӱ�");
            OP.PoolDestroy(this.gameObject);
        }
    }
}
