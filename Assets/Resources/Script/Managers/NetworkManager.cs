using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using static ObjectPooler;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("DisconnectPanel")]
    [SerializeField]  GameObject DisconnectPanel;
    [SerializeField]   TMP_InputField NicknameInput;
    [Header("RoomPanel")]
    [SerializeField]   GameObject RoomPanel;
    [SerializeField]   GameObject InitGameBtn;
    [SerializeField] GameObject background;
    [SerializeField] GameObject winRegamePanel;
    [SerializeField] GameObject loseRegamePanel;
    [SerializeField] GameObject hostWaitingTxt;
    [SerializeField] GameObject disconnectTxt;
    [SerializeField] GameObject explainTxt;
    [SerializeField] GameObject countDownTxt;
    [SerializeField] TMP_Text countTxt;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField]  GameObject[] NicknameTexts;
    [SerializeField]   float[] cameraZPos;
    public PhotonView PV;
    public GameObject CurObj;
    public static NetworkManager instance;
    public int leftRight = 1;
    public bool startGame = false;
    public int count = 1;
    float time = 1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        Screen.SetResolution(720, 540, false);
        NicknameInput.characterLimit = 6;
        startGame = false;
    }

    private void Update()
    {
        if (!PhotonNetwork.InRoom) return;
        if(RoomPanel.activeSelf) countTxt.text = "���� �ο� ��\n( " + PhotonNetwork.CurrentRoom.PlayerCount.ToString() +" / 2 )" ;
        if(startGame && PhotonNetwork.CurrentRoom.PlayerCount == 2) CheckWhoAreWinner();
     }

    void CheckWhoAreWinner()
    {
        if (time <= 0)
        {
            GameObject[] Players = GameObject.FindGameObjectsWithTag("Player"); //1�ʿ� �� ���� scene�� ����Ⱑ �� �� �ִ��� üũ
            if (Players.Length != 2)
            {
                if(loseRegamePanel.activeSelf == false)
                    winRegamePanel.SetActive(true);
                startGame = false;
            }
            else time = 1f;
        }
        else time -= Time.deltaTime;
    }

    public void Connect()
    {
        PhotonNetwork.LocalPlayer.NickName = NicknameInput.text;
        PhotonNetwork.ConnectUsingSettings();//���� ����
    }

    public override void OnConnectedToMaster() => PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 2}, null);

    public override void OnJoinedRoom()
    {
        ShowPanel(RoomPanel);
        if (Master()) InitGameBtn.SetActive(true);
        else
        {
            hostWaitingTxt.SetActive(true);
        }
    }

    void ShowPanel(GameObject curPanel)
    {
        DisconnectPanel.SetActive(false);
        curPanel.SetActive(true);
    }

    bool Master()
    {
        return PhotonNetwork.LocalPlayer.IsMasterClient;
    }

    public void Init()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount != 2)
        {
            StartCoroutine("ShowExplain");
            return;
        }
        InitGameBtn.SetActive(false);
        hostWaitingTxt.SetActive(false);
        PV.RPC("InitGames", RpcTarget.AllViaServer);
    }

    IEnumerator ShowExplain()
    {
        explainTxt.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        explainTxt.SetActive(false);
    }

    [PunRPC]
    void InitGames()
    {
        RoomPanel.SetActive(false);
        OP.PrePoolInstantiate();
        for (int i = 0; i < 2; i++)
        {
            NicknameTexts[i].SetActive(true);
            NicknameTexts[i].GetComponent<TMP_Text>().text = PhotonNetwork.PlayerList[i].NickName;
        }
        int playerNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        Transform spawnPoint = spawnPoints[playerNumber - 1];
        GameObject Pl = PhotonNetwork.Instantiate("Prefabs/Player", spawnPoint.position, spawnPoint.rotation);
        Camera.main.transform.rotation = Quaternion.Euler(0, 0, cameraZPos[playerNumber - 1]);
        leftRight = Master() ? 1 : -1;
        background.GetComponent<Background>().enabled = true;
        StartCoroutine("CountDown");
    }

    IEnumerator CountDown()
    {
        countDownTxt.SetActive(true);
        countDownTxt.GetComponent<TMP_Text>().text = "3";
        yield return new WaitForSeconds(1.0f);
        countDownTxt.GetComponent<TMP_Text>().text = "2";
        yield return new WaitForSeconds(1.0f);
        countDownTxt.GetComponent<TMP_Text>().text = "1";
        yield return new WaitForSeconds(1.0f);
        countDownTxt.SetActive(false);
        startGame = true;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) //���� �����ϰ� ������ ����ǵ��� �ٲٱ�
    {
        if(startGame)
        {
            disconnectTxt.SetActive(true); //�÷��̾ �����ٴ� ���� �˷���
            StartCoroutine("Restart");
        }
    }
    IEnumerator Restart()
    {
        yield return new WaitForSeconds(3.0f);
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("SampleScene");
    }

    public void Restart2()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("SampleScene");
    }
}
