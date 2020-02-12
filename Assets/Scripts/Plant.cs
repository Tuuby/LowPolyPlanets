using UnityEngine;
using UnityEngine.UI;

public class Plant
{
    public Polygon position;

    //State
    //@0 = just planted
    //@1 = fully grown
    //@-1 = dried out
    //@-2 = to be destroyed
    sbyte state;
    public sbyte lastState = -3;

    public float temperaturePref { get; }
    public float temperatureTolerance { get; set; }
    public float humidityPref { get; }
    public float humidityTolerance { get; set; }
    public float sulfurTolerance { get; set; }
    public byte groundTypePref { get; }
    public string plantName { get; }

    public GameObject oldGameObject;
    public Text statusText;
    public GameObject fullyGrownPrefab;

    private float lifetime;

    public Plant(Polygon position, Text status, PlantFamily family)
    {
        statusText = status;
        this.position = position;
        state = 0;

        this.temperaturePref = family.temperaturePref;
        this.humidityPref = family.humidityPref;
        this.groundTypePref = family.groundTypePref;
        this.fullyGrownPrefab = family.plantPrefab;

        temperatureTolerance = family.temperatureTolerance;
        humidityTolerance = family.humidityTolerance;
        sulfurTolerance = family.sulfurTolerance;
    }

    public Plant(Polygon position, Text status, float temperaturePref, float humidityPref, byte groundTypePref)
    {
        statusText = status;
        this.position = position;
        state = 0;

        this.temperaturePref = temperaturePref;
        this.humidityPref = humidityPref;
        this.groundTypePref = groundTypePref;

        temperatureTolerance = 10;
        humidityTolerance = 10;
        sulfurTolerance = 15;
    }

    //Depricated constructor for standard plants
    public Plant(Polygon position, Text status)
    {
        statusText = status;
        this.position = position;
        state = 0;

        temperaturePref = 15;
        humidityPref = 50;
        groundTypePref = 2;
    }

    public sbyte getState()
    {
        return state;
    }

    public sbyte checkGroundCompatibility(float temperature, float humidity, float sulfurLevel, byte groundType)
    {
        lifetime += Time.deltaTime;

        if (state == 1 || state == -1)
        {
            if (lifetime >= 20 && state == -1)
                state = -2;
            return state;
        } 
        else
        {
            if (groundType == groundTypePref)
            {
                if (lifetime >= 5)
                {
                    if (Mathf.Abs(temperature - temperaturePref) <= temperatureTolerance)
                    {
                        if (Mathf.Abs(humidity - humidityPref) <= humidityTolerance)
                        {
                            if (sulfurLevel <= sulfurTolerance)
                            {
                                state = 1;
                                lifetime = 21;
                            }
                            else
                            {
                                state = -1;
                                statusText.text = "To much sulfur";
                            }
                        }
                        else
                        {
                            state = -1;
                            statusText.text = "Not the right humidity";
                        }
                    }
                    else
                    {
                        state = -1;
                        statusText.text = "Not the right temperature";
                    }
                }
            } 
            else
            {
                if (lifetime >= 10)
                {
                    if (Mathf.Abs(temperature - temperaturePref) <= temperatureTolerance)
                    {
                        if (Mathf.Abs(humidity - humidityPref) <= humidityTolerance)
                        {
                            if (sulfurLevel <= sulfurTolerance)
                            {
                                state = 1;
                                lifetime = 21;
                            }
                            else
                            {
                                state = -1;
                                statusText.text = "To much sulfur, you monkey";
                            }
                        }
                        else
                        {
                            state = -1;
                            statusText.text = "Not the right humidity";
                        }
                    }
                    else 
                    {
                        state = -1;
                        statusText.text = "Not the right temperature";
                    }
                }
            }
        }

        return state;
    }
}