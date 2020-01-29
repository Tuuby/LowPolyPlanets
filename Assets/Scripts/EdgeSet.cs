using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeSet : HashSet<Edge>
{
    public Dictionary<int, Vector3> GetInwardDirections(List<Vector3> vertexPositions)
    {
        Dictionary<int, Vector3> inwardDirections = new Dictionary<int, Vector3>();
        Dictionary<int, int> numContributions = new Dictionary<int, int>();

        foreach (Edge edge in this)
        {
            Vector3 innerVertexPosition = vertexPositions[edge.m_InwardDirectionVertex];

            Vector3 edgePosA = vertexPositions[edge.m_InnerVerts[0]];
            Vector3 edgePosB = vertexPositions[edge.m_InnerVerts[1]];
            Vector3 edgeCenter = Vector3.Lerp(edgePosA, edgePosB, 0.5f);
            Vector3 innerVector = (innerVertexPosition - edgeCenter).normalized;

            for (int i = 0; i < 2; i++)
            {
                int edgeVertex = edge.m_InnerVerts[i];
                if (inwardDirections.ContainsKey(edgeVertex))
                {
                    inwardDirections[edgeVertex] += innerVector;
                    numContributions[edgeVertex]++;
                }
                else
                {
                    inwardDirections.Add(edgeVertex, innerVector);
                    numContributions.Add(edgeVertex, 1);
                }
            }
        }

        foreach (KeyValuePair<int, int> kvp in numContributions)
        {
            int vertexIndex = kvp.Key;
            int contributionsToThisVortex = kvp.Value;
            inwardDirections[vertexIndex] = (inwardDirections[vertexIndex] / contributionsToThisVortex).normalized;
        }

        return inwardDirections;
    }

    public void Split(List<int> oldVertices, List<int> newVertices)
    {
        foreach(Edge edge in this)
        {
            for (int i = 0; i < 2; i++)
            {
                edge.m_InnerVerts[i] = newVertices[oldVertices.IndexOf(edge.m_OuterVerts[i])];
            }
        }
    }

    public List<int> GetUniqueVertices()
    {
        List<int> vertices = new List<int>();
        foreach (Edge edge in this)
        {
            foreach (int vert in edge.m_OuterVerts)
            {
                if (!vertices.Contains(vert))
                    vertices.Add(vert);
            }
        }

        return vertices;
    }
}
