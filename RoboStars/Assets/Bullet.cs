using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour
{
    [SerializeField] private BulletInfo info;
    private Rigidbody rb;
    private PhotonView pv;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
        info.render = gameObject;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!pv.IsMine) return;

        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponentInParent<PlayerSetting>().TakeDamage(info.damage);
            PhotonNetwork.Destroy(gameObject);
        }
    }

    public void StartMove(Vector3 dir)
    {
        rb.velocity = dir * info.speed;
    }


}
