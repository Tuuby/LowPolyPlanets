using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    public Text PlantNameText;
    public Text TemperatureToleranceText;
    public Text TemperaturePreferenceText;
    public Text HumidityToleranceText;
    public Text HumidityPreferenceText;
    public Text SulfurToleranceText;
    public Text statusText;
    public PlantFamily PlantToShow;
    public Player player;

    // Update is called once per frame
    void Update()
    {
        if (PlantToShow == null)
        {
            return;
        }

        PlantNameText.text = PlantToShow.plantName;
        TemperatureToleranceText.text = string.Format("{0:0.00}°C", PlantToShow.temperatureTolerance);
        TemperaturePreferenceText.text = string.Format("Temperature - {0:0.00}°C", PlantToShow.temperaturePref);
        HumidityToleranceText.text = string.Format("{0:0.}%", PlantToShow.humidityTolerance);
        HumidityPreferenceText.text = string.Format("Humidity - {0:0.}%", PlantToShow.humidityPref);
        SulfurToleranceText.text = string.Format("{0:0.0}%", PlantToShow.sulfurTolerance);
    }

    public void IncreasePlantTemp()
    {
        if (PlantToShow == null)
        {
            return;
        }

        if (PlantToShow.temperatureBonusLvl * 5 <= player.currency)
        {
            player.currency -= PlantToShow.temperatureBonusLvl * 5;
            PlantToShow.temperatureTolerance += PlantToShow.temperatureBonusLvl * 5;
            PlantToShow.temperatureBonusLvl++;
        }
        else
        {
            statusText.text = "Not enough currency";
        }
    }

    public void IncreasePlantHumidity()
    {
        if (PlantToShow == null)
        {
            return;
        }

        if (PlantToShow.humidityBonusLvl * 5 <= player.currency)
        {
            player.currency -= PlantToShow.humidityBonusLvl * 5;
            PlantToShow.humidityTolerance += PlantToShow.humidityBonusLvl * 5;
            PlantToShow.humidityBonusLvl++;
        }
        else
        {
            statusText.text = "Not enough currency";
        }
    }

    public void IncreasePlantSulfur()
    {
        if (PlantToShow == null)
        {
            return;
        }

        if (PlantToShow.sulfurBonusLvl * 5 <= player.currency)
        {
            player.currency -= PlantToShow.sulfurBonusLvl * 5;
            PlantToShow.sulfurTolerance += PlantToShow.sulfurBonusLvl * 5;
            PlantToShow.sulfurBonusLvl++;
        }
        else
        {
            statusText.text = "Not enough currency";
        }
    }

    public void SelectPlantType(PlantFamily family)
    {
        PlantToShow = family;
    }
}
