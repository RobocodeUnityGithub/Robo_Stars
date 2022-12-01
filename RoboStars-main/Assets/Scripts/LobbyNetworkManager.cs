using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Random = UnityEngine.Random;


public class LobbyNetworkManager : MonoBehaviourPunCallbacks
{
    public static LobbyNetworkManager Instance; //сінглтон
    [SerializeField] private TMP_Text waitBattleText;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        WindowsManager.Layout.OpenLayuot("Loading");
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        WindowsManager.Layout.OpenLayuot("MainMenu");
    }

    public void ToBattleButton()
    {
        WindowsManager.Layout.OpenLayuot("AutomaticBattle");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        if (returnCode == (short)ErrorCode.NoRandomMatchFound)
        {
            waitBattleText.text = "No matches found. We are creating a new room";
            CreateNewRoom();
        }
    }

    private string RoomNameGenerator()
    {
        short codeLengths = 12;
        string roomCode = null;
        for (int i = 0; i < codeLengths; i++)
        {
            char symbol = (char)Random.Range(65, 91);
            roomCode += symbol; //room a -> 827979109065
        }

        return roomCode;
    }

    private void CreateNewRoom()
    {
        RoomOptions currentRoom = new RoomOptions();
        currentRoom.IsOpen = true;
        currentRoom.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(RoomNameGenerator(), currentRoom);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        if (returnCode == (short)ErrorCode.GameIdAlreadyExists)
        {
            CreateNewRoom();
        }
    }

    public override void OnCreatedRoom()
    {
        waitBattleText.text = "Waiting for the second player";
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        if (PhotonNetwork.IsMasterClient)
        {
            return;
        }

        waitBattleText.text = "The battle begin! Get ready!";
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        Room currenRoom = PhotonNetwork.CurrentRoom;
        currenRoom.IsOpen = false;
        waitBattleText.text = "The battle begin! Get ready!";
        Invoke(nameof(LoadingGameMap), 3f);
    }

    private void LoadingGameMap()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public void StopFindButton()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        WindowsManager.Layout.OpenLayuot("MainMenu");
    }

    void Update()
    {
        
    }
}
