using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polygon
{
    public List<int> m_Vertices;
    public List<Vector2> m_UVs;
    public List<Polygon> m_Neighbours;
    public Color32 m_Color;
    public bool m_SmoothNormals;
    public float temperature;
    public float humidity;
    public byte groundType;
    public float sulfurLevel;

    public Polygon(int a, int b, int c)
    {
        m_Vertices = new List<int>() { a, b, c };
        m_Neighbours = new List<Polygon>();
        m_UVs = new List<Vector2>() { Vector2.zero, Vector2.zero, Vector2.zero };
        m_SmoothNormals = true;

        m_Color = new Color32(255, 0, 255, 255);
    }

    public void calculateTemperature(Vector3 centre, int temperatureOffset)
    {
        temperature = NoiseTest.Noise.CalcPixel3D((int)(centre.x * 100), (int)(centre.y * 100), (int)(centre.z * 100), 0.005f);
        temperature /= 2;
        temperature -= temperatureOffset;
    }

    public void calculateHumidity(Vector3 centre)
    {
        humidity = NoiseTest.Noise.CalcPixel3D((int)(centre.x * 100), (int)(centre.y * 100), (int)(centre.z * 100), 0.005f);
    }

    public void calculateSulfurLevel(Vector3 centre)
    {
        sulfurLevel = NoiseTest.Noise.CalcPixel3D((int)(centre.x * 100), (int)(centre.y * 100), (int)(centre.z * 100), 0.005f);
        sulfurLevel -= 100;
        if (sulfurLevel < 0)
            sulfurLevel = 0;
    }

    public bool isNeighbourOf(Polygon other_poly)
    {
        int shared_vertices = 0;
        foreach (int vertex in m_Vertices)
        {
            if (other_poly.m_Vertices.Contains(vertex))
                shared_vertices++;
        }

        return shared_vertices == 2;
    }

    public void ReplaceNeighbour(Polygon oldNeighbour, Polygon newNeighbour)
    {
        for (int i = 0; i < m_Neighbours.Count; i++)
        {
            if (oldNeighbour == m_Neighbours[i])
            {
                m_Neighbours[i] = newNeighbour;
                return;
            }
        }
    }
}
