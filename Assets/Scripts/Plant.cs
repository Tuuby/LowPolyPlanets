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
    sbyte state;
    public sbyte lastState = -2;

    float temperaturePref;
    float temperatureTolerance;
    float humidityPref;
    float humidityTolerance;
    float sulfurTolerance;
    byte groundTypePref;

    private float lifetime;

    public Plant(Polygon position)
    {
        this.position = position;
        state = 0;
    }

    public sbyte checkGroundCompatibility(float temperature, float humidity, float sulfurLevel, byte groundType)
    {
        lifetime += Time.deltaTime;

        //TODO: make more checks if plant can grow

        if (lifetime >= 5)
        {
            if (groundType == groundTypePref)
            {
                state = 1;
                lifetime = 11;
            }
        }
        else if (lifetime >= 10)
        {
            if (groundType == 3)
            {
                state = -1;
            }
            else if (lifetime >= 10)
            {
                state = 1;
                lifetime = 11;
            }
            else
            {
                state = 0;
            }
        }

        return state;
    }
}