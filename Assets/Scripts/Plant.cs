using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    float temperaturePref;
    public static float temperatureTolerance = 100;
    float humidityPref;
    public static float humidityTolerance = 100;
    public static float sulfurTolerance = 150;
    byte groundTypePref;

    public GameObject oldGameObject;

    private float lifetime;

    public Plant(Polygon position)
    {
        this.position = position;
        state = 0;

        temperaturePref = 15;
        humidityPref = 40;
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
                                Debug.Log("To much sulfur, you monkey");
                            }
                        }
                        else
                        {
                            state = -1;
                            Debug.Log("Not the right humidity");
                        }
                    }
                    else
                    {
                        state = -1;
                        Debug.Log("Not the right temperature");
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
                                Debug.Log("To much sulfur, you monkey");
                            }
                        }
                        else
                        {
                            state = -1;
                            Debug.Log("Not the right humidity");
                        }
                    }
                    else 
                    {
                        state = -1;
                        Debug.Log("Not the right temperature");
                    }
                }
            }
        }

        return state;
    }
}