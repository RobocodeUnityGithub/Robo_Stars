using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatisticComponent : MonoBehaviour
{
    protected int playerLevel; 
    protected int currentEXP;
    protected int EXPtoNextLevel;
    protected const int START_EXP_VALUE = 500;
    protected int countBuffPoints;

    public int GetCountBuffPoints()
    {
        return countBuffPoints;
    }

    public virtual void TakeCountBuffPoints()
    {
        countBuffPoints--;
        SaveData();
    }

    public virtual void Awake() 
    {
        playerLevel = PlayerPrefs.GetInt("PlayerLevel");
        if (playerLevel == 0) playerLevel = 1;
        currentEXP = PlayerPrefs.GetInt("CurrentEXP");
        countBuffPoints = PlayerPrefs.GetInt("CountBuffPoints");
        UpdateEXPToNextLevel();
    }
    protected void UpdateEXPToNextLevel()
    {
        EXPtoNextLevel = START_EXP_VALUE * playerLevel;
    }

    protected void SaveData()
    {
        PlayerPrefs.SetInt("PlayerLevel", playerLevel);
        PlayerPrefs.SetInt("CurrentEXP", currentEXP);
        PlayerPrefs.SetInt("CountBuffPoints", countBuffPoints);
    }

    


}

