using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Text progressText;

    public int temperatureBonus;
    int temperatureBonusLvl;
    public int humidtyBonus;
    int humidityBonusLvl;
    public int sulfurBonus;
    int sulfurBonusLvl;

    private void Start()
    {
        progressText.text = "0%";
        temperatureBonus = 0;
        humidtyBonus = 0;
        sulfurBonus = 0;
        temperatureBonusLvl = 1;
        humidityBonusLvl = 1;
        sulfurBonusLvl = 1;
    }

    public void updateProgress(float progress)
    {
        progressText.text = (int)(progress * 100) + "%";
    }

    public void increaseTemperatureBonus()
    {
        temperatureBonus += temperatureBonusLvl * 5;
        Plant.temperatureTolerance += temperatureBonus;
        temperatureBonusLvl++;
    }

    public void increaseHumidityBonus()
    {
        humidtyBonus += humidtyBonus * 5;
        Plant.humidityTolerance += humidtyBonus;
        humidityBonusLvl++;
    }

    public void increaseSulfurBonus()
    {
        sulfurBonus += sulfurBonusLvl * 5;
        Plant.sulfurTolerance += sulfurBonus;
        sulfurBonusLvl++;
    }
}
