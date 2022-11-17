using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerSetting : MonoBehaviourPunCallbacks
{
    [SerializeField] private int healt;
    [SerializeField] private int maxHealt;
    [SerializeField] private Slider healtBar;
    private PhotonView pv;

    private void Awake()
    {
        pv = GetComponentInParent<PhotonView>();
    }

    private void Start()
    {
        healt = maxHealt;
        healtBar.value = healt;
    }

    public void TakeDamage(int value)
    {
        pv.RPC("UpdateHealt", RpcTarget.All, value);
    }

    [PunRPC]
    public void UpdateHealt(int value)
    {
        healt -= value;

        if (healt <= 0)
        {
            healt = maxHealt;
            transform.GetComponentInChildren<PlayerController>().Respawn();
        }
        healtBar.value = healt;
    }

}
