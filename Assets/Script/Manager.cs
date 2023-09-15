using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Manager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    Transform[] spawnPoints;
    [SerializeField]
    float[] cameraZPos;
    private void Awake()
    {
        Screen.SetResolution(1080, 1920, false);
        PhotonNetwork.ConnectUsingSettings();//서버 연결
    }
    public override void OnConnectedToMaster() => PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 2}, null);
    public override void OnJoinedRoom()
    {
        int playerNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        Debug.Log(playerNumber);
        Transform spawnPoint = spawnPoints[playerNumber - 1];
        GameObject Pl = PhotonNetwork.Instantiate("Player", spawnPoint.position, spawnPoint.rotation);
        Camera.main.transform.rotation = Quaternion.Euler(0, 0, cameraZPos[playerNumber - 1]);
    }
}
