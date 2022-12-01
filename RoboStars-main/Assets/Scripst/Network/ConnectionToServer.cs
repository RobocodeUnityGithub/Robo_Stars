using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class ConnectionToServer : MonoBehaviourPunCallbacks
{
    public static ConnectionToServer Instance;
    [SerializeField] TMP_InputField inputRoomName;
    [SerializeField] TMP_Text roomName;
    [SerializeField] Transform transformRoomList;
    [SerializeField] GameObject roomItemPrefab;
    [SerializeField] Transform transformPlayerList;
    [SerializeField] GameObject playerListItem;
    [SerializeField] GameObject startGameButton;

    [SerializeField] GameObject mapSelector;
    [SerializeField] TMP_Text mapNameText;
    [SerializeField] string[] allGameMap;
    private int maxMapIndex;
    private int currentMapIndex = 1;

    public void NextMap()
    {
        currentMapIndex++;
        Debug.Log("next currentMapIndex " + currentMapIndex);
        if (currentMapIndex >= maxMapIndex) currentMapIndex = 1;
        mapNameText.text = allGameMap[currentMapIndex];
    }
    public void PrivMap()
    {
        currentMapIndex--;
        Debug.Log("priv currentMapIndex " + currentMapIndex);
        if (currentMapIndex <= 1) currentMapIndex = maxMapIndex;
        mapNameText.text = allGameMap[currentMapIndex];
    }
    private void Awake()
    {
        maxMapIndex = SceneManager.sceneCountInBuildSettings - 1;
        Instance = this;
        PhotonNetwork.ConnectUsingSettings();
        mapNameText.text = allGameMap[1];
    }
    public void StartGameLevel()
    {
        PhotonNetwork.LoadLevel(currentMapIndex);
    }


    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        Debug.Log(PhotonNetwork.CountOfPlayersOnMaster);
        PhotonNetwork.NickName = "Player" + Random.Range(0, 1000);

        PhotonNetwork.AutomaticallySyncScene = true;
    }


    public override void OnJoinedRoom()
    {
        WindowsManager.Layout.OpenLayuot("GameRoom");
        roomName.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;
        foreach (Transform trns in transformPlayerList)
        {
            Destroy(trns.gameObject);
        }
        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(playerListItem, transformPlayerList).
               GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            startGameButton.SetActive(true);
            mapSelector.SetActive(true);
        }
        else
        {
            startGameButton.SetActive(false);
            mapSelector.SetActive(false);
        }
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            startGameButton.SetActive(true);
            mapSelector.SetActive(true);
        }
        else
        {
            startGameButton.SetActive(false);
            mapSelector.SetActive(false);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItem, transformPlayerList).
            GetComponent<PlayerListItem>().SetUp(newPlayer);
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trns in transformRoomList)
        {
            Destroy(trns.gameObject);
        }

        for (int i = 0; i < roomList.Count; i++)
        {
             Instantiate(roomItemPrefab, transformRoomList).
                GetComponent<RoomItem>().SetUp(roomList[i]);
        }
    }
    // added
    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
    }

    public void CreateNewRoom()
    {
        if (string.IsNullOrEmpty(inputRoomName.text)) return;
        PhotonNetwork.CreateRoom(inputRoomName.text);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        WindowsManager.Layout.OpenLayuot("MainMenu");
    }

    public override void OnJoinedLobby()
    {
        WindowsManager.Layout.OpenLayuot("MainMenu");
        Debug.Log("Connected to Lobby");
    }

    public void ConnectToRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }


}
