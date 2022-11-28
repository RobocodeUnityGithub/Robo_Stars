using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatisticComponent : MonoBehaviour
{
    protected int playerLevel; // protected дозволить використовувати цю змінну під час наслідування
    protected int currentEXP;
    protected int EXPtoNextLevel;
    protected const int START_EXP_VALUE = 500; // базове значення кількості очок для переход уна наступний рівень
    // із кожним новим рівнем це значення буде збільшуватись по формулі START_EXP_VALUE * playerLevel

    public virtual void Awake() // virtual дозволить змінити код в середені Awake під час наслідування
    {
        playerLevel = PlayerPrefs.GetInt("PlayerLevel");
        if (playerLevel == 0) playerLevel = 1;
        currentEXP = PlayerPrefs.GetInt("CurrentEXP");
        UpdateEXPToNextLevel();
    }
    protected void UpdateEXPToNextLevel()
    {
        EXPtoNextLevel = START_EXP_VALUE * playerLevel;
    }
}

