using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using static ObjectPooler;

public class PlayerController : MonoBehaviour
{
    public PhotonView PV;
    //[SerializeField]
    //GameObject NicknameText;
    [SerializeField]
    Sprite[] texture;
    [SerializeField]
    float[] XPos;
    [SerializeField]
    GameObject bullet;
    float speed;
    float maxShotDelay = 0.5f;
    float curShotDelay;

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
        if (PV.IsMine)
        {
            Move();
            Fire();
            Reload();
        }
    }
        void Move()
        {
            float h = Input.GetAxis("Horizontal") * NetworkManager.instance.leftRight ;
            transform.position += new Vector3(h * 5 * Time.deltaTime, 0, 0);
        }

        void Fire()
        {
            if (!Input.GetKey(KeyCode.Space))
                return;
            if (curShotDelay < maxShotDelay)
                return;
        GameObject bullet = OP.PoolInstantiate("Bullet", transform.position, transform.rotation);
        bullet.transform.position = transform.position;
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.velocity = new Vector2(0, NetworkManager.instance.leftRight) * 3 ;
        curShotDelay = 0;
        }

        void Reload()
        {
            curShotDelay += Time.deltaTime;
        }
}
