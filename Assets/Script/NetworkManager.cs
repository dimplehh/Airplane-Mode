using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("DisconnectPanel")]
    public GameObject DisconnectPanel;
    public TMP_InputField NicknameInput;
    [Header("RoomPanel")]
    public GameObject RoomPanel;
    public GameObject InitGameBtn;
    [SerializeField]
    Transform[] spawnPoints;
    [SerializeField]
    float[] cameraZPos;
    public PhotonView PV;

    private void Awake()
    {
        Screen.SetResolution(540, 540, false);
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
        PV.RPC("InitGames", RpcTarget.AllViaServer);
    }
    [PunRPC]
    void InitGames()
    {
        RoomPanel.SetActive(false);
        int playerNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        Debug.Log(playerNumber);
        Transform spawnPoint = spawnPoints[playerNumber - 1];
        GameObject Pl = PhotonNetwork.Instantiate("Player", spawnPoint.position, spawnPoint.rotation);
        Camera.main.transform.rotation = Quaternion.Euler(0, 0, cameraZPos[playerNumber - 1]);
    }
}
