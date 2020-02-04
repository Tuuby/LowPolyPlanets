using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Text progressText;
    public Text temperatureText;
    public Text humidityText;
    public Text sulfurText;
    public Text currencyText;

    private float lifetime;

    int currency;

    public int temperatureBonus;
    int temperatureBonusLvl;
    public int humidityBonus;
    int humidityBonusLvl;
    public int sulfurBonus;
    int sulfurBonusLvl;

    private void Start()
    {
        progressText.text = "0%";
        temperatureBonus = 0;
        humidityBonus = 0;
        sulfurBonus = 0;
        temperatureBonusLvl = 1;
        humidityBonusLvl = 1;
        sulfurBonusLvl = 1;

        currency = 10;

        temperatureText.text = "Tolerance: 10 + " + temperatureBonus;
        humidityText.text = "Tolerance: 10 + " + humidityBonus;
        sulfurText.text = "Tolerance: 15 + " + sulfurBonus;
    }

    private void Update()
    {
        lifetime += Time.deltaTime;
        currencyText.text = currency.ToString();

        if (lifetime >= 10)
        {
            currency += 5;
            lifetime = 0;
        }
    }

    public bool plantPlanted(int cost)
    {
        if (currency >= cost)
        {
            currency -= cost;
            return true;
        }
        else
            return false;
    }

    public void plantFinishes()
    {
        currency += 25;
    }

    public void updateProgress(float progress)
    {
        progressText.text = (int)(progress * 100) + "%";
    }

    public void increaseTemperatureBonus()
    {
        if (temperatureBonusLvl * 5 <= currency)
        {
            currency -= temperatureBonusLvl * 5;
            temperatureBonus += temperatureBonusLvl * 5;
            Plant.temperatureTolerance += temperatureBonus;
            temperatureBonusLvl++;

            temperatureText.text = "Tolerance: 10 + " + temperatureBonus;
        }
        else
        {
            Debug.Log("Not enough currency");
        }
    }

    public void increaseHumidityBonus()
    {
        if (humidityBonusLvl * 5 <= currency)
        {
            currency -= humidityBonusLvl * 5;
            humidityBonus += humidityBonusLvl * 5;
            Plant.humidityTolerance += humidityBonus;
            humidityBonusLvl++;

            humidityText.text = "Tolerance: 10 + " + humidityBonus;
        }
        else
        {
            Debug.Log("Not enough currency");
        }
    }

    public void increaseSulfurBonus()
    {
        if (sulfurBonusLvl * 5 <= currency)
        {
            currency -= sulfurBonusLvl * 5;
            sulfurBonus += sulfurBonusLvl * 5;
            Plant.sulfurTolerance += sulfurBonus;
            sulfurBonusLvl++;

            sulfurText.text = "Tolerance: 15 + " + sulfurBonus;
        }
        else
        {
            Debug.Log("Not enough currency");
        }
        
    }
}
