using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class NickNameDisplay : MonoBehaviour
{
    [SerializeField] private PhotonView pv;
    [SerializeField] private TMP_Text nickName;
    

    void Awake()
    {
        if (pv.IsMine) Destroy(gameObject);
    }
    void Start()
    {
        nickName.text = pv.Owner.NickName;
    }
}
