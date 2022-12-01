using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;


public class GameNetworkManager : MonoBehaviourPunCallbacks
{
    public UnityEvent OnGameOver;
    public UnityEvent OnGameWin;
    [SerializeField] private GameObject allPlayerUI;
    private PhotonView _pv;

    private void Awake()
    {
        _pv = gameObject.GetPhotonView();
    }
    private void Start()
    {
        if (!_pv.IsMine)
        {
            allPlayerUI.SetActive(false);
            return;
        }
    }
    public void OutOfBattle()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }
}
