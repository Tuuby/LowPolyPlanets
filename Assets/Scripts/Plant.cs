﻿using System.Collections;
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

        temperaturePref = 15;
        temperatureTolerance = 25;
        humidityPref = 60;
        humidityTolerance = 30;
        sulfurTolerance = 50;
        groundTypePref = 2;
    }

    public sbyte getState()
    {
        return state;
    }

    public sbyte checkGroundCompatibility(float temperature, float humidity, float sulfurLevel, byte groundType)
    {
        if (state == 1 || state == -1)
            return state;

        lifetime += Time.deltaTime;

        //TODO: make more checks if plant can grow
        if (Mathf.Abs(temperature - temperaturePref) <= temperatureTolerance)
        {
            if (Mathf.Abs(humidity - humidityPref) <= humidityTolerance)
            {
                if (sulfurLevel <= sulfurTolerance)
                {
                    if (groundType == groundTypePref)
                    {
                        if (lifetime >= 5)
                        {
                            state = 1;
                            lifetime = 11;
                        }
                        else
                            state = 0;
                    }
                    else
                    {
                        if (lifetime >= 10)
                        {
                            state = 1;
                            lifetime = 11;
                        }
                        else
                            state = 0;
                    }
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

        return state;
    }
}