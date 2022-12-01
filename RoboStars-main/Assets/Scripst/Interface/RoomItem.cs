using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    [SerializeField] private Text roomName;
    RoomInfo info;
    public void SetUp(RoomInfo info)
    {
        this.info = info;
        roomName.text = info.Name;
    }
    public void OnClick()
    {
        ConnectionToServer.Instance.JoinRoom(this.info);
    }
}
