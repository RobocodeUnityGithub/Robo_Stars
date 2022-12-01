using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class PlayerBuff : MonoBehaviour
{
    [SerializeField] private int addHealt;
    private int currentCoutnAddHealt;

    private void Awake()
    {
        LoadBuffs();
    }

    public int GetCurrentCoutnAddHealt()
    {
        return currentCoutnAddHealt;
    }

    public void BuyBuff()
    {
        PlayerStatisticComponent playerStatisticComponent = GetComponent<PlayerStatisticComponent>();

        if (playerStatisticComponent.GetCountBuffPoints() > 0)
        {
            playerStatisticComponent.TakeCountBuffPoints();
            currentCoutnAddHealt += addHealt;
            SaveBuffs();
        }
    }

    private void LoadBuffs()
    {
        currentCoutnAddHealt = PlayerPrefs.GetInt("CurrentCoutnAddHealt");
    }

    private void SaveBuffs()
    {
        PlayerPrefs.SetInt("CurrentCoutnAddHealt", currentCoutnAddHealt);
    }
}
