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
    [SerializeField] GameObject hostWaitingTxt;
    [SerializeField] GameObject disconnectTxt;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField]  GameObject[] NicknameTexts;
    [SerializeField]   float[] cameraZPos;
    public PhotonView PV;
    public GameObject CurObj;

    private void Awake()
    {
        Screen.SetResolution(720, 540, false);
        NicknameInput.characterLimit = 6;
    }

    public void Connect()
    {
        PhotonNetwork.LocalPlayer.NickName = NicknameInput.text;
        PhotonNetwork.ConnectUsingSettings();//서버 연결
    }

    public override void OnConnectedToMaster() => PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 2}, null);

    public override void OnJoinedRoom()
    {
        ShowPanel(RoomPanel);
        if (Master()) InitGameBtn.SetActive(true);
        else hostWaitingTxt.SetActive(true);
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
        if (PhotonNetwork.CurrentRoom.PlayerCount != 2) return;
        InitGameBtn.SetActive(false);
        hostWaitingTxt.SetActive(false);
        PV.RPC("InitGames", RpcTarget.AllViaServer);
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
        background.GetComponent<Background>().enabled = true;
    }

    void Update()
    {
        if (!PhotonNetwork.InRoom)
            return;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        disconnectTxt.SetActive(true); //플레이어가 나갔다는 것을 알려줌
        StartCoroutine("Restart");
    }
    IEnumerator Restart()
    {
        yield return new WaitForSeconds(3.0f);
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("SampleScene");
    }
}
