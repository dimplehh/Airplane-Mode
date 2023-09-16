using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using static ObjectPooler;

public class Star : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().eatStar = true;
            OP.PoolDestroy(this.gameObject);
        }
    }
}
