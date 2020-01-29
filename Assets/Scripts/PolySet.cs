using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolySet : HashSet<Polygon>
{
    public PolySet() { }
    public PolySet(PolySet source) : base(source) { }

    public int m_StitchedVertexThreshold = -1;

    public EdgeSet CreateEdgeSet()
    {
        EdgeSet edgeSet = new EdgeSet();
        foreach (Polygon poly in this)
        {
            foreach (Polygon neighbour in poly.m_Neighbours)
            {
                if (this.Contains(neighbour))
                    continue;

                Edge edge = new Edge(poly, neighbour);
                edgeSet.Add(edge);
            }
        }

        return edgeSet;
    }

    public List<int> GetUniqueVertices()
    {
        List<int> verts = new List<int>();
        foreach (Polygon poly in this)
        {
            foreach (int vert in poly.m_Vertices)
            {
                if (!verts.Contains(vert))
                    verts.Add(vert);
            }
        }

        return verts;
    }

    public void ApplyAmbientOcclusionTerm(float AOForOriginalVerts, float AOForNewVerts)
    {
        foreach (Polygon poly in this)
        {
            for (int i = 0; i < 3; i++)
            {
                float aoTerm = (poly.m_Vertices[i] > m_StitchedVertexThreshold) ? AOForNewVerts : AOForOriginalVerts;

                Vector2 uv = poly.m_UVs[i];
                uv.y = aoTerm;
                poly.m_UVs[i] = uv;
            }
        }
    }

    public PolySet RemoveEdges()
    {
        var newSet = new PolySet();

        var edgeSet = CreateEdgeSet();

        var edgeVertices = edgeSet.GetUniqueVertices();

        foreach (Polygon poly in this)
        {
            bool polyTouchesEdge = false;

            for (int i = 0; i < 3; i++)
            {
                if (edgeVertices.Contains(poly.m_Vertices[i]))
                {
                    polyTouchesEdge = true;
                    break;
                }
            }

            if (polyTouchesEdge)
                continue;

            newSet.Add(poly);
        }

        return newSet;
    }

    public void ApplyColor(Color32 c)
    {
        foreach (Polygon poly in this)
            poly.m_Color = c;
    }
}
