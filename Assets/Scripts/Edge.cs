using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{
    public Polygon m_InnerPoly;
    public Polygon m_OuterPoly;

    public List<int> m_OuterVerts;
    public List<int> m_InnerVerts;

    public int m_InwardDirectionVertex;

    public Edge(Polygon inner_poly, Polygon outer_poly)
    {
        m_InnerPoly = inner_poly;
        m_OuterPoly = outer_poly;
        m_OuterVerts = new List<int>(2);
        m_InnerVerts = new List<int>(2);

        foreach (int vertex in inner_poly.m_Vertices)
        {
            if (outer_poly.m_Vertices.Contains(vertex))
                m_InnerVerts.Add(vertex);
            else
                m_InwardDirectionVertex = vertex;
        }

        if (m_InnerVerts[0] == inner_poly.m_Vertices[0] && m_InnerVerts[1] == inner_poly.m_Vertices[2])
        {
            int tmp = m_InnerVerts[0];
            m_InnerVerts[0] = m_InnerVerts[1];
            m_InnerVerts[1] = tmp;
        }

        m_OuterVerts = new List<int>(m_InnerVerts);
    }
}
