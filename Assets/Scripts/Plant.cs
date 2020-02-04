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

    float temperaturePref { get; }
    public static float temperatureTolerance = 10;
    float humidityPref { get; }
    public static float humidityTolerance = 20;
    public static float sulfurTolerance = 15;
    byte groundTypePref;

    public GameObject oldGameObject;
    public Text statusText;

    private float lifetime;

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