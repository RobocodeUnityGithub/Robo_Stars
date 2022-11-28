using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuPlayerStatisticComponent : PlayerStatisticComponent
{
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text currentEXP_Text;
    [SerializeField] private Image fillEXP_Area;
    private void Start()
    {
        levelText.text = $"Level: {playerLevel}";
        currentEXP_Text.text = $"{currentEXP} / {EXPtoNextLevel}";
        fillEXP_Area.fillAmount = (float)currentEXP / (float)EXPtoNextLevel;
    }
}
