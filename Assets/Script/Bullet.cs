using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using static ObjectPooler;

public class Bullet : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
    int dir;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!PV.IsMine && collision.tag == "Player" && collision.GetComponent<PhotonView>().IsMine)
        {
            collision.GetComponent<PlayerController>().Hit(0.34f);
            OP.PoolDestroy(this.gameObject);
        }
    }
}
