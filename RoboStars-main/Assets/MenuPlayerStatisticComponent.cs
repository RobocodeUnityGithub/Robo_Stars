using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuPlayerStatisticComponent : PlayerStatisticComponent
{
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text currentEXP_Text;
    [SerializeField] private Image fillEXP_Area;
    [SerializeField] private TMP_Text countBuffPointsText;

    private void Start()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        levelText.text = $"Level: {playerLevel}";
        currentEXP_Text.text = $"{currentEXP} / {EXPtoNextLevel}";
        fillEXP_Area.fillAmount = (float)currentEXP / (float)EXPtoNextLevel;
        countBuffPointsText.text = $"Update({countBuffPoints})";
    }

    public override void TakeCountBuffPoints()
    {
        base.TakeCountBuffPoints();
        UpdateUI();
    }

}
