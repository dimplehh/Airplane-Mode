using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using static ObjectPooler;
using static UIManager;

public class GameManager : MonoBehaviourPunCallbacks
{
    #region public
    public PhotonView PV;
    [HideInInspector] public GameObject CurObj;
    [HideInInspector] public static GameManager instance;
    [HideInInspector] public int leftRight = 1;
    [HideInInspector] public bool startGame = false;
    [HideInInspector] public int count = 1;
    #endregion public
    #region private
    [SerializeField] GameObject background;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject[] NicknameTexts;
    [SerializeField] float[] cameraZPos;
    UnityEngine.Quaternion[] flipPlayer = { Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 180, 0) };
    float time = 1f;
    #endregion
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        Screen.SetResolution(720, 540, false);
        startGame = false;
    }

    private void Update()
    {
        if (!PhotonNetwork.InRoom) return;
        if(UI.RoomPanel.activeSelf) UI.countTxt.text = "대기실 인원 수\n( " + PhotonNetwork.CurrentRoom.PlayerCount.ToString() +" / 2 )" ;
        if(startGame && PhotonNetwork.CurrentRoom.PlayerCount == 2) CheckWhoAreWinner();
     }

    void CheckWhoAreWinner()
    {
        if (time <= 0)
        {
            GameObject[] Players = GameObject.FindGameObjectsWithTag("Player"); //1초에 한 번씩 scene에 비행기가 몇 개 있는지 체크
            if (Players.Length != 2)
            {
                if(UI.loseRegamePanel.activeSelf == false)
                    UI.winRegamePanel.SetActive(true);
                startGame = false;
            }
            else time = 1f;
        }
        else time -= Time.deltaTime;
    }

    public void Connect()
    {
        PhotonNetwork.LocalPlayer.NickName = UI.NicknameInput.text;
        PhotonNetwork.ConnectUsingSettings();//서버 연결
    }

    public override void OnConnectedToMaster() => PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 2}, null);

    public override void OnJoinedRoom()
    {
        ShowPanel(UI.RoomPanel);
        if (Master()) UI.InitGameBtn.SetActive(true);
        else
        {
            UI.hostWaitingTxt.SetActive(true);
        }
    }

    void ShowPanel(GameObject curPanel)
    {
        UI.DisconnectPanel.SetActive(false);
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
        UI.InitGameBtn.SetActive(false);
        UI.hostWaitingTxt.SetActive(false);
        PV.RPC("InitGames", RpcTarget.AllViaServer);
    }

    IEnumerator ShowExplain()
    {
        UI.explainTxt.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        UI.explainTxt.SetActive(false);
    }

    [PunRPC]
    void InitGames()
    {
        UI.RoomPanel.SetActive(false);
        OP.PrePoolInstantiate();
        UI.NicknameTxt1.GetComponent<TMP_Text>().text = PhotonNetwork.PlayerList[0].NickName;
        UI.NicknameTxt2.GetComponent<TMP_Text>().text = PhotonNetwork.PlayerList[1].NickName;

        int playerNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        Transform spawnPoint = spawnPoints[playerNumber - 1];
        GameObject Pl = PhotonNetwork.Instantiate("Prefabs/Player", spawnPoint.position, spawnPoint.rotation);
        if(playerNumber == 2) Pl.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
        Camera.main.transform.rotation = Quaternion.Euler(0, 0, cameraZPos[playerNumber - 1]);

        leftRight = Master() ? 1 : -1;
        background.GetComponent<Background>().enabled = true;
        UI.GameUI.SetActive(true);
        StartCoroutine("CountDown");
    }

    IEnumerator CountDown()
    {
        UI.countDownTxt.SetActive(true);
        for (int i = 3; i > 0; i--)
        {
            UI.countDownTxt.GetComponent<TMP_Text>().text = i.ToString();
            yield return new WaitForSeconds(1.0f);
        }
        UI.countDownTxt.SetActive(false);
        startGame = true;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) //게임 실행하고 나서만 실행되도록 바꾸기
    {
        if(startGame)
        {
            UI.disconnectTxt.SetActive(true); //플레이어가 나갔다는 것을 알려줌
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
