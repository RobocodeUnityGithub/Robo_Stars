using UnityEngine;
using TMPro;
using Photon.Pun;

public class NetworkStatistic : MonoBehaviour
{
    private TMP_Text onlinePlayers;
    void Start()
    {
        onlinePlayers = gameObject.GetComponent<TMP_Text>();
    }
    void Update()
    {
        onlinePlayers.text = 
            "Connected Players: " + PhotonNetwork.CountOfPlayers.ToString();
    }
}
