using UnityEngine;
using Photon.Pun;
using System.IO;
public class PlayerManager : MonoBehaviourPunCallbacks
{
    private PhotonView pnView;
    private GameObject controller;
    private GameObject destroyFX;
    private void Awake()
    {
        pnView = GetComponent<PhotonView>();
    }
    void Start()
    {
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
        if (pnView.IsMine)
        {
            CreateController();
        }
    }
    private void CreateController()
    {
        Transform point = SpawnManager.Instance.GetSpawPoint();
        controller =  PhotonNetwork.Instantiate(Path.Combine("PlayerController"),
            point.position, point.rotation, 0, new object[] {pnView.ViewID});
    }
    public void Die(Vector3 position)
    {
        destroyFX = PhotonNetwork.Instantiate(Path.Combine("DestroyFX"), position,
            Quaternion.identity, 0, new object[] {pnView.ViewID});
        PhotonNetwork.Destroy(controller);
        CreateController();
    }
}
