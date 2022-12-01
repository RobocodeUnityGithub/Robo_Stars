using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using ExitGames.Client.Photon;


public class PlayerSetting : MonoBehaviourPunCallbacks
{
    [SerializeField] private int healt;
    [SerializeField] private int maxHealt;
    [SerializeField] private Slider healtBar;
    private PhotonView pv;
    private GameNetworkManager gameManager;
    private const byte GAME_IS_WIN = 0;

    private void Awake()
    {
        pv = GetComponentInParent<PhotonView>();
        gameManager = gameObject.GetComponentInParent<GameNetworkManager>();
        //тут треба реалізувати (так щоб значення змінилося у всіх) maxHealt += gameObject.GetComponentInParent<PlayerBuff>().GetCurrentCoutnAddHealt();
    }
    private void Start()
    {
        healt = maxHealt;
        healtBar.value = healt;
    }
    public override void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnNetworkEventCome;
    }
    public override void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnNetworkEventCome;
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
            if (!pv.IsMine) return;
            SendDeadEvent();
            gameManager.OnGameOver.Invoke();
        }
        healtBar.value = healt;
    }
    private void OnNetworkEventCome(EventData obj)
    {
        if (obj.Code == GAME_IS_WIN)
        {
            if (!pv.IsMine) return;
            gameManager.OnGameWin.Invoke();
        }
    }
    private void SendDeadEvent()
    {
        object[] datas = null; // масив даних, який можна передати через нашу подію. Він пустий, оскільки нас цікавить тільки активація події.

        PhotonNetwork.RaiseEvent(GAME_IS_WIN, datas, RaiseEventOptions.Default, SendOptions.SendUnreliable);
        // Активуємо подію із нашим ключем.
        // В Datas у нас пусто
        // RiseEventOption дозволяє вказати чи буде кешуватись ця подія.
    }
}
