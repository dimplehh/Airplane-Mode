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
    [SerializeField]
    private StageData stageData;
    [SerializeField]
    GameObject NicknameText;
    public Image healthImage;
    [SerializeField]
    Sprite[] texture;
    [SerializeField]
    float[] XPos;
    float curShotDelay;

    private void Awake()
    {
        NicknameText.GetComponent<TMP_Text>().color = PV.IsMine?Color.green:Color.red;
        NicknameText.GetComponent<TMP_Text>().text = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;
        NicknameText.GetComponent<RectTransform>().localRotation = PV.IsMine? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0,0,180);
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

    private void LateUpdate()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, stageData.LimitMin.x, stageData.LimitMax.x),
                                                                    Mathf.Clamp(transform.position.y, stageData.LimitMin.y, stageData.LimitMax.y));
    }

    void Move()
        {
            float h = Input.GetAxis("Horizontal") * GameManager.instance.leftRight ;
            transform.position += new Vector3(h * 5 * Time.deltaTime, 0, 0);
        }

        void Fire()
        {
            if (!Input.GetKey(KeyCode.Space))
                return;
            if (curShotDelay < LevelManager.instance.maxShotDelay)
                return;
        if (!GameManager.instance.startGame)
            return;
        SoundManager.instance.SfxPlaySound(0, transform.position);
        GameObject bullet = OP.PoolInstantiate("Prefabs/Bullet", transform.position, transform.rotation);
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.velocity = new Vector2(0, GameManager.instance.leftRight) * LevelManager.instance.curSpeed ;
        curShotDelay = 0;
        }

        void Reload()
        {
            curShotDelay += Time.deltaTime;
        }

    public void Hit(float _damage)
    {
        PV.RPC("TakeHitRPC", RpcTarget.All, _damage);
        if(healthImage.fillAmount <= 0)
        {
            PV.RPC("DestroyRPC", RpcTarget.AllBuffered);
            GameObject.Find("Canvas").transform.Find("loseRegamePanel").gameObject.SetActive(true);
            SoundManager.instance.SfxPlaySound(2, transform.position);
        }
    }
    [PunRPC]
    public void TakeHitRPC(float _damage)
    {
        SoundManager.instance.SfxPlaySound(1, transform.position);
        healthImage.fillAmount -= _damage;
        StartCoroutine("ImHit");
    }

    IEnumerator ImHit()
    {
        for(int i = 0; i < 2; i++)
        {
            transform.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.7f);
            yield return new WaitForSeconds(0.05f);
            transform.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
            yield return new WaitForSeconds(0.05f);
        }
    }

    [PunRPC]
    void DestroyRPC() => Destroy(gameObject);

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(healthImage.fillAmount);
        }
        else
        {
            healthImage.fillAmount = (float)stream.ReceiveNext();
        }
    }
}
