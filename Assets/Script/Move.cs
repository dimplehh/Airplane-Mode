using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class Move : MonoBehaviour
{
    public PhotonView PV;
    //[SerializeField]
    //GameObject NicknameText;
    [SerializeField]
    Sprite[] texture;
    [SerializeField]
    float[] XPos;

    private void Awake()
    {
        //NicknameText.GetComponent<TMP_Text>().color = PV.IsMine?Color.green:Color.red;
        gameObject.GetComponent<SpriteRenderer>().sprite = PV.IsMine? texture[0]: texture[1];
    }

    private void Start()
    {
    }

    private void Update()
    {
        if(PV.IsMine)
        {
            float h = Input.GetAxis("Horizontal");
            transform.position += new Vector3(h * 5 * Time.deltaTime, 0,0);

        }
    }
}
