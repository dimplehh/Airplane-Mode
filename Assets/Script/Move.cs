using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Move : MonoBehaviour
{
    public PhotonView PV;

    private void Update()
    {
        if(PV.IsMine)
        {
            float h = Input.GetAxis("Horizontal");
            transform.position += new Vector3(h * 5 * Time.deltaTime, 0,0);

        }
    }
}
